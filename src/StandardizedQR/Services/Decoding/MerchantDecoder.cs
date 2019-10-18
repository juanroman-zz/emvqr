using StandardizedQR.Utils;
using StandardizedQR.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;

namespace StandardizedQR.Services.Decoding
{
    internal class MerchantDecoder : IPayloadDecoder<MerchantPayload>
    {
        private static readonly int[] _parentTagsIdentifiers =
        {
            // Merchant Account Information
            02, 03, 04, 05, 06, 07, 08, 09, 10,
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
            51,
            // Additional Data Template
            62,
            // Language Template
            64,
            // Unreserved Template
            80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
            90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
        };

        public MerchantPayload BuildPayload(ICollection<Tlv> tlvs)
        {
            var merchantPayload = new MerchantPayload
            {
                AdditionalData = new MerchantAdditionalData(),
                MerchantInformation = new MerchantInfoLanguageTemplate(),
            };

            var properties = typeof(MerchantPayload).GetProperties();
            foreach (var property in properties)
            {
                ReflectAndBind(merchantPayload, tlvs, property);
            }

            DecodeAccountInformation(tlvs, merchantPayload);
            DecodeUnreservedTemplate(tlvs, merchantPayload);

            // Before validation we can remove additional data and the merchant language template if no data for them was available.
            // They are optional fields but if they are not populated then validation fails.
            if (null == merchantPayload.AdditionalData.AdditionalConsumerDataRequest
                && null == merchantPayload.AdditionalData.BillNumber
                && null == merchantPayload.AdditionalData.CustomerLabel
                && null == merchantPayload.AdditionalData.LoyaltyNumber
                && null == merchantPayload.AdditionalData.MobileNumber
                && null == merchantPayload.AdditionalData.PurposeOfTransaction
                && null == merchantPayload.AdditionalData.ReferenceLabel
                && null == merchantPayload.AdditionalData.StoreLabel
                && null == merchantPayload.AdditionalData.TerminalLabel)
            {
                merchantPayload.AdditionalData = null;
            }

            if (null == merchantPayload.MerchantInformation.LanguagePreference
                && null == merchantPayload.MerchantInformation.MerchantCityAlternateLanguage
                && null == merchantPayload.MerchantInformation.MerchantNameAlternateLanguage)
            {
                merchantPayload.MerchantInformation = null;
            }

            return merchantPayload;
        }

        public string ValidateCrc(string qrData)
        {
            var data = qrData.Substring(0, qrData.Length - 4);
            var crc = new CRC.Crc(CRC.CrcStdParams.StandartParameters[CRC.CrcAlgorithms.Crc16CcittFalse]).ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            var crcValue = crc.ToHex(true).GetLast(4);

            if (0 != StringComparer.Ordinal.Compare(crcValue, qrData.GetLast(4).ToUpperInvariant()))
            {
                throw new SecurityException(LibraryResources.InvalidCrc);
            }

            return crcValue;
        }

        public ICollection<Tlv> DecodeQR(string qrData)
        {
            var tlvs = new List<Tlv>();

            /// Remove CRC
            var data = qrData.Substring(0, qrData.Length - 8);

            // Parse root nodes
            ParseTLVs(data, tlvs);

            // Parse Child Nodes
            var allowedParentNodes = tlvs.Where(e => _parentTagsIdentifiers.Contains(e.Tag));
            foreach (var item in allowedParentNodes)
            {
                ParseTLVs(item.Value, item.ChildNodes);
            }

            return tlvs.AsReadOnly();
        }

        private void ParseTLVs(string data, ICollection<Tlv> tlvs)
        {
            for (int index = 0; index < data.Length; index++)
            {
                var tag = data.Substring(index, 2);
                index += 2;

                var schemaLength = data.Substring(index, 2);
                if (!int.TryParse(schemaLength, out int length))
                {
                    throw new InvalidOperationException(LibraryResources.FailedToDecode);
                }
                index += 2;

                if (data.Length - 4 < length)
                {
                  break;
                }

                var value = data.Substring(index, length);
                index += length - 1;

                tlvs.Add(new Tlv(tag, length, value));
            }
        }

        private void ReflectAndBind<T>(T instance, ICollection<Tlv> tlvs, PropertyInfo property)
        {
            var customAttribute = property
                                .GetCustomAttributes(typeof(EmvSpecificationAttribute), false)
                                .FirstOrDefault();
            if (customAttribute != null && customAttribute is EmvSpecificationAttribute emvSpecification)
            {
                var tlv = tlvs.FirstOrDefault(e => e.Tag == emvSpecification.Id);
                if (null != tlv)
                {
                    if (!emvSpecification.IsParent)
                    {
                        property.SetAndCastValue(instance, tlv.Value);
                    }
                    else
                    {
                        var properties = property.PropertyType.GetProperties();
                        var itemInstance = property.GetValue(instance);

                        foreach (var childProperty in properties)
                        {
                            ReflectAndBind(itemInstance, tlv.ChildNodes, childProperty);
                        }
                    }
                }
            }
        }

        private void DecodeAccountInformation(ICollection<Tlv> tlvs, MerchantPayload merchantPayload)
        {
            var merchantAccountInfoTlvs = tlvs.Where(e => e.Tag >= 2 && e.Tag <= 51);
            if (merchantAccountInfoTlvs.Any())
            {
                merchantPayload.MerchantAccountInformation = new MerchantAccountInformationDictionary();
                foreach (var tlv in merchantAccountInfoTlvs)
                {
                    var accountInfo = new MerchantAccountInformation();

                    // Visa and MasterCard simply have card numbers in their reserved space (02 and 04 for example - Issue #2).
                    // If there are no child nodes, then the data for this tag is not a TLV string, we could probably assume it's the GlobalUniqueIdentifier since it's a required field.
                    if (!tlv.ChildNodes.Any())
                    {
                        accountInfo.GlobalUniqueIdentifier = tlv.Value;
                    }
                    else
                    {
                        var globalUniqueIdentifierTlv = tlv.ChildNodes.FirstOrDefault(t => t.Tag == 0);
                        if (null != globalUniqueIdentifierTlv)
                        {
                            accountInfo.GlobalUniqueIdentifier = globalUniqueIdentifierTlv.Value;

                            var paymentNetworkSpecificTlvs = tlv.ChildNodes.Where(e => e.Tag >= 1 && e.Tag <= 99);
                            if (paymentNetworkSpecificTlvs.Any())
                            {
                                accountInfo.PaymentNetworkSpecific = new Dictionary<int, string>();
                                foreach (var item in paymentNetworkSpecificTlvs)
                                {
                                    accountInfo.PaymentNetworkSpecific.Add(item.Tag, item.Value);
                                }
                            }
                        }
                    }
                    
                    merchantPayload.MerchantAccountInformation.Add(tlv.Tag, accountInfo);
                }
            }
        }

        private void DecodeUnreservedTemplate(ICollection<Tlv> tlvs, MerchantPayload merchantPayload)
        {
            var unreservedTemplateTlvs = tlvs.Where(e => e.Tag >= 80 && e.Tag <= 99);
            if (unreservedTemplateTlvs.Any())
            {
                merchantPayload.UnreservedTemplate = new MerchantUnreservedDictionary();
                foreach (var tlv in unreservedTemplateTlvs)
                {
                    var unreservedTemplate = new MerchantUnreservedTemplate();
                    var globalUniqueIdentifierTlv = tlv.ChildNodes.FirstOrDefault(t => t.Tag == 0);
                    if (null != globalUniqueIdentifierTlv)
                    {
                        unreservedTemplate.GlobalUniqueIdentifier = globalUniqueIdentifierTlv.Value;

                        var contextSpecificTlvs = tlv.ChildNodes.Where(e => e.Tag >= 1 && e.Tag <= 99);
                        if (contextSpecificTlvs.Any())
                        {
                            unreservedTemplate.ContextSpecificData = new Dictionary<int, string>();
                            foreach (var item in contextSpecificTlvs)
                            {
                                unreservedTemplate.ContextSpecificData.Add(item.Tag, item.Value);
                            }
                        }

                        merchantPayload.UnreservedTemplate.Add(tlv.Tag, unreservedTemplate);
                    }
                }
            }
        }
    }
}
