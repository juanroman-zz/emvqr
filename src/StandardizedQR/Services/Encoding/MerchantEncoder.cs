using StandardizedQR.CRC;
using StandardizedQR.Utils;
using StandardizedQR.Validation;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StandardizedQR.Services.Encoding
{
    public class MerchantEncoder : IPayloadEncoding<MerchantPayload>
    {
        public string GeneratePayload(MerchantPayload payload)
        {
            var sb = new StringBuilder();
            sb.Append(EncodeProperty(nameof(MerchantPayload.PayloadFormatIndicator), payload.PayloadFormatIndicator));
            sb.Append(EncodeProperty(nameof(MerchantPayload.PointOfInitializationMethod), payload.PointOfInitializationMethod));

            if (null != payload.MerchantAccountInformation)
            {
                foreach (var merchantAccountInfo in payload.MerchantAccountInformation)
                {
                    var merchantInfoBuilder = new StringBuilder();

                    merchantInfoBuilder.Append(EncodeProperty(typeof(MerchantAccountInformation).GetProperty("GlobalUniqueIdentifier"), merchantAccountInfo.Value.GlobalUniqueIdentifier));
                    foreach (var paymentNetworkItem in merchantAccountInfo.Value.PaymentNetworkSpecific)
                    {
                        merchantInfoBuilder.Append(EncodeKeyPair(paymentNetworkItem.Key, paymentNetworkItem.Value));
                    }

                    sb.AppendFormat("{0:D2}{1:D2}{2}", merchantAccountInfo.Key, merchantInfoBuilder.Length, merchantInfoBuilder);
                }
            }

            sb.Append(EncodeProperty(nameof(MerchantPayload.MerchantCategoryCode), payload.MerchantCategoryCode));
            sb.Append(EncodeProperty(nameof(MerchantPayload.CountyCode), payload.CountyCode));
            sb.Append(EncodeProperty(nameof(MerchantPayload.MerchantName), payload.MerchantName));
            sb.Append(EncodeProperty(nameof(MerchantPayload.MerchantCity), payload.MerchantCity));
            sb.Append(EncodeProperty(nameof(MerchantPayload.PostalCode), payload.PostalCode));

            if (null != payload.MerchantInformation)
            {
                var languateTemplateBuilder = new StringBuilder();
                languateTemplateBuilder.Append(EncodeProperty(typeof(MerchantInfoLanguageTemplate).GetProperty(nameof(MerchantInfoLanguageTemplate.LanguagePreference)), payload.MerchantInformation.LanguagePreference));
                languateTemplateBuilder.Append(EncodeProperty(typeof(MerchantInfoLanguageTemplate).GetProperty(nameof(MerchantInfoLanguageTemplate.MerchantNameAlternateLanguage)), payload.MerchantInformation.MerchantNameAlternateLanguage));
                languateTemplateBuilder.Append(EncodeProperty(typeof(MerchantInfoLanguageTemplate).GetProperty(nameof(MerchantInfoLanguageTemplate.MerchantCityAlternateLanguage)), payload.MerchantInformation.MerchantCityAlternateLanguage));

                sb.Append(EncodeProperty(nameof(MerchantPayload.MerchantInformation), languateTemplateBuilder.ToString()));
            }

            sb.Append(EncodeProperty(nameof(MerchantPayload.TransactionAmount), payload.TransactionAmount));
            sb.Append(EncodeProperty(nameof(MerchantPayload.TransactionCurrency), payload.TransactionCurrency));
            sb.Append(EncodeProperty(nameof(MerchantPayload.TipOrConvenienceIndicator), payload.TipOrConvenienceIndicator));
            sb.Append(EncodeProperty(nameof(MerchantPayload.ValueOfConvenienceFeeFixed), payload.ValueOfConvenienceFeeFixed));
            sb.Append(EncodeProperty(nameof(MerchantPayload.ValueOfConvenienceFeePercentage), payload.ValueOfConvenienceFeePercentage));

            if (null != payload.AdditionalData)
            {
                var additionalDataBuilder = new StringBuilder();
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.BillNumber)), payload.AdditionalData.BillNumber));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.MobileNumber)), payload.AdditionalData.MobileNumber));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.StoreLabel)), payload.AdditionalData.StoreLabel));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.LoyaltyNumber)), payload.AdditionalData.LoyaltyNumber));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.ReferenceLabel)), payload.AdditionalData.ReferenceLabel));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.CustomerLabel)), payload.AdditionalData.CustomerLabel));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.TerminalLabel)), payload.AdditionalData.TerminalLabel));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.PurposeOfTransaction)), payload.AdditionalData.PurposeOfTransaction));
                additionalDataBuilder.Append(EncodeProperty(typeof(MerchantAdditionalData).GetProperty(nameof(MerchantAdditionalData.AdditionalConsumerDataRequest)), payload.AdditionalData.AdditionalConsumerDataRequest));

                sb.Append(EncodeProperty(nameof(MerchantPayload.AdditionalData), additionalDataBuilder.ToString()));
            }

            if (null != payload.UnreservedTemplate)
            {
                foreach (var unreservedTemplateItem in payload.UnreservedTemplate)
                {
                    var merchantInfoBuilder = new StringBuilder();

                    merchantInfoBuilder.Append(EncodeProperty(typeof(MerchantAccountInformation).GetProperty("GlobalUniqueIdentifier"), unreservedTemplateItem.Value.GlobalUniqueIdentifier));
                    foreach (var dataItem in unreservedTemplateItem.Value.ContextSpecificData)
                    {
                        merchantInfoBuilder.Append(EncodeKeyPair(dataItem.Key, dataItem.Value));
                    }

                    sb.AppendFormat("{0:D2}{1:D2}{2}", unreservedTemplateItem.Key, merchantInfoBuilder.Length, merchantInfoBuilder);
                }
            }

            /*
             * Add CRC
             * 
             * The checksum shall be calculated according to [ISO/IEC 13239] using the polynomial '1021' (hex) and initial 
             * value 'FFFF' (hex). The data over which the checksum is calculated shall cover all data objects, including their 
             * ID, Length and Value, to be included in the QR Code, in their respective order, as well as the ID and Length of 
             * the CRC itself (but excluding its Value).
             */
            sb.Append("6304"); //// {id:63}{length:04}
            var crc16ccittFalseParameters = CrcStdParams.StandartParameters[CrcAlgorithms.Crc16CcittFalse];
            var crc = new Crc(crc16ccittFalseParameters).ComputeHash(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
            sb.Append(crc.ToHex(true).GetLast(4));

            return sb.ToString();
        }

        private string EncodeProperty<T>(string propertyName, T propertyValue)
        {
            var property = typeof(MerchantPayload)
                .GetProperty(propertyName);

            return EncodeProperty(property, propertyValue);
        }

        private string EncodeProperty<T>(PropertyInfo property, T propertyValue)
        {
            var emvSpecAttribute = (EmvSpecificationAttribute)property
                .GetCustomAttributes(typeof(EmvSpecificationAttribute), false)
                .First();
      
            string value = EncodePropertyValue(propertyValue);
      
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            
            string id = emvSpecAttribute.Id.ToString("D2");
            string length = value.Length.ToString("D2");

            return $"{id}{length}{value}";
        }

        private string EncodeKeyPair(int id, string value) => string.Format(CultureInfo.InvariantCulture, "{0:D2}{1:D2}{2}", id, value.Length, value);

        private string EncodePropertyValue<T>(T propertyValue)
        {
            // Value can be  int, int?, decimal?, string or null
            if (typeof(T) == typeof(int))
            {
                var intValue = (int)(object)propertyValue;
                return intValue.ToString("D2");
            }
            else if (propertyValue is int?)
            {
                var nullableInt = propertyValue as int?;
                if (nullableInt.HasValue)
                {
                    return nullableInt.Value.ToString("D2");
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (propertyValue is decimal?)
            {
                var nullableDecimal = propertyValue as decimal?;
                if (nullableDecimal.HasValue)
                {
                    return nullableDecimal.Value.ToString("#.00");
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (propertyValue is string)
            {
                return propertyValue.ToString();
            }
            else if (propertyValue == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
