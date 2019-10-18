using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StandardizedQR.XUnitTests
{
    public class MerchantPayloadUnitTests
    {
        [Fact]
        public void FullEncodeAndDecode()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var merchantPayload = MerchantPayload
                .CreateDynamic(globalUniqueIdentifier, 4111, Iso4217Currency.MexicoPeso.Value.NumericCode, Iso3166Countries.Mexico, "Chocolate Powder", "Mexico City")
                .WithAlternateLanguage(Iso639Languages.SpanishCastilian, "Chocolate en Polvo", "CDMX")
                .WithTransactionAmount(34.95m)
                .WithTipByUser()
                .WithAdditionalData(
                    billNumber: "1234",
                    mobileNumber: "5512341234",
                    storeLabel: "The large store",
                    loyaltyNumber: "A12341234",
                    referenceLabel: "***",
                    terminalLabel: "T12341",
                    purposeOfTransaction: "We do commerce",
                    additionalConsumerDataRequest: "AME")
                .WithUnreservedTemplate(globalUniqueIdentifier, new Dictionary<int, string>
                {
                    {1, "Some value" },
                    {2, "Another value" }
                });
            merchantPayload.PostalCode = "12345";

            var qr = merchantPayload.GeneratePayload();

            merchantPayload = MerchantPayload.FromQR(qr);
            Assert.Equal(globalUniqueIdentifier, merchantPayload.MerchantAccountInformation.First().Value.GlobalUniqueIdentifier);
            Assert.Equal(4111, merchantPayload.MerchantCategoryCode);
            Assert.Equal(Iso4217Currency.MexicoPeso.Value.NumericCode, merchantPayload.TransactionCurrency);
            Assert.Equal(Iso3166Countries.Mexico, merchantPayload.CountyCode);
            Assert.Equal("Chocolate Powder", merchantPayload.MerchantName);
            Assert.Equal("Mexico City", merchantPayload.MerchantCity);
            Assert.Equal(Iso639Languages.SpanishCastilian, merchantPayload.MerchantInformation.LanguagePreference);
            Assert.Equal("Chocolate en Polvo", merchantPayload.MerchantInformation.MerchantNameAlternateLanguage);
            Assert.Equal("CDMX", merchantPayload.MerchantInformation.MerchantCityAlternateLanguage);
            Assert.Equal(34.95m, merchantPayload.TransactionAmount);
            Assert.Equal(1, merchantPayload.TipOrConvenienceIndicator);
            Assert.Equal("1234", merchantPayload.AdditionalData.BillNumber);
            Assert.Equal("5512341234", merchantPayload.AdditionalData.MobileNumber);
            Assert.Equal("The large store", merchantPayload.AdditionalData.StoreLabel);
            Assert.Equal("A12341234", merchantPayload.AdditionalData.LoyaltyNumber);
            Assert.Equal("***", merchantPayload.AdditionalData.ReferenceLabel);
            Assert.Equal("T12341", merchantPayload.AdditionalData.TerminalLabel);
            Assert.Equal("We do commerce", merchantPayload.AdditionalData.PurposeOfTransaction);
            Assert.Equal("AME", merchantPayload.AdditionalData.AdditionalConsumerDataRequest);
            Assert.Equal(globalUniqueIdentifier, merchantPayload.UnreservedTemplate.First().Value.GlobalUniqueIdentifier);
            Assert.Equal("Some value", merchantPayload.UnreservedTemplate.First().Value.ContextSpecificData[1]);
            Assert.Equal("Another value", merchantPayload.UnreservedTemplate.First().Value.ContextSpecificData[2]);
            Assert.Equal("12345", merchantPayload.PostalCode);
            Assert.NotNull(merchantPayload.CRC);
        }

        [Fact]
        public void DecodeQR()
        {
            var qrData = "00020101021229300012D156000000000510A93FO3230Q31280012D15600000001030812345678520441115802CN5914BEST TRANSPORT6007BEIJING64200002ZH0104最佳运输0202北京540523.7253031565502016233030412340603***0708A60086670902ME91320016A0112233449988770708123456786304A13A";
            var payload = MerchantPayload.FromQR(qrData);

            Assert.Equal(1, payload.PayloadFormatIndicator);
            Assert.Equal(12, payload.PointOfInitializationMethod);
            Assert.Equal("A13A", payload.CRC);
            Assert.Equal(4111, payload.MerchantCategoryCode);
            Assert.Equal("CN", payload.CountyCode);
            Assert.Equal(23.72m, payload.TransactionAmount);
            Assert.Equal(156, payload.TransactionCurrency);
            Assert.Equal(1, payload.TipOrConvenienceIndicator);

            Assert.Equal("D15600000000", payload.MerchantAccountInformation[29].GlobalUniqueIdentifier);
            Assert.Equal("A93FO3230Q", payload.MerchantAccountInformation[29].PaymentNetworkSpecific[5]);
            Assert.Equal("D15600000001", payload.MerchantAccountInformation[31].GlobalUniqueIdentifier);
            Assert.Equal("12345678", payload.MerchantAccountInformation[31].PaymentNetworkSpecific[3]);

            Assert.Equal("ZH", payload.MerchantInformation.LanguagePreference);
            Assert.Equal("最佳运输", payload.MerchantInformation.MerchantNameAlternateLanguage);
            Assert.Equal("北京", payload.MerchantInformation.MerchantCityAlternateLanguage);

            Assert.Equal("1234", payload.AdditionalData.StoreLabel);
            Assert.Equal("***", payload.AdditionalData.CustomerLabel);
            Assert.Equal("A6008667", payload.AdditionalData.TerminalLabel);
            Assert.Equal("ME", payload.AdditionalData.AdditionalConsumerDataRequest);

            Assert.Equal("A011223344998877", payload.UnreservedTemplate[91].GlobalUniqueIdentifier);
            Assert.Equal("12345678", payload.UnreservedTemplate[91].ContextSpecificData[7]);
        }

        [Fact]
        public void DecodeQR1()
        {
            var qrData = "0002010102110213404587173785204155326311010619815204829953039365802GH5909CIB GHANA6005ACCRA622407088656730603088656730663041437";
            var payload = MerchantPayload.FromQR(qrData);

            Assert.Equal(1, payload.PayloadFormatIndicator);
            Assert.Equal(11, payload.PointOfInitializationMethod);
            Assert.Equal("1437", payload.CRC);
            Assert.Equal(8299, payload.MerchantCategoryCode);
            Assert.Equal(936, payload.TransactionCurrency);
            Assert.Equal("CIB GHANA", payload.MerchantName);
            Assert.Equal("ACCRA", payload.MerchantCity);
            Assert.Equal("GH", payload.CountyCode);

            Assert.Equal("4045871737852", payload.MerchantAccountInformation[2].GlobalUniqueIdentifier);
            Assert.Equal("532631101061981", payload.MerchantAccountInformation[4].GlobalUniqueIdentifier);

            Assert.Equal("86567306", payload.AdditionalData.StoreLabel);
            Assert.Equal("86567306", payload.AdditionalData.TerminalLabel);
        }

        [Fact]
        public void DecodeQR2()
        {
            var qrData = "0002010102110213404587194150404155326311017361105204581153039365802GH5913SUSAN ALLOTEY6005ACCRA622407080407330503080407330563049EE4";
            var payload = MerchantPayload.FromQR(qrData);

            Assert.Equal(1, payload.PayloadFormatIndicator);
            Assert.Equal(11, payload.PointOfInitializationMethod);
            Assert.Equal("9EE4", payload.CRC);
            Assert.Equal(5811, payload.MerchantCategoryCode);
            Assert.Equal(936, payload.TransactionCurrency);
            Assert.Equal("SUSAN ALLOTEY", payload.MerchantName);
            Assert.Equal("ACCRA", payload.MerchantCity);
            Assert.Equal("GH", payload.CountyCode);

            Assert.Equal("4045871941504", payload.MerchantAccountInformation[2].GlobalUniqueIdentifier);
            Assert.Equal("532631101736110", payload.MerchantAccountInformation[4].GlobalUniqueIdentifier);

            Assert.Equal("04073305", payload.AdditionalData.StoreLabel);
            Assert.Equal("04073305", payload.AdditionalData.TerminalLabel);
        }

        [Fact]
        public void DecodeQR3()
        {
            var qrData = "0002010102110213404587568745904155326311155509945204625353039365802GH5915MAXMART LIMITED6005ACCRA62240708620037450308620037456304C913";
            var payload = MerchantPayload.FromQR(qrData);

            Assert.Equal(1, payload.PayloadFormatIndicator);
            Assert.Equal(11, payload.PointOfInitializationMethod);
            Assert.Equal("C913", payload.CRC);
            Assert.Equal(6253, payload.MerchantCategoryCode);
            Assert.Equal(936, payload.TransactionCurrency);
            Assert.Equal("MAXMART LIMITED", payload.MerchantName);
            Assert.Equal("ACCRA", payload.MerchantCity);
            Assert.Equal("GH", payload.CountyCode);

            Assert.Equal("4045875687459", payload.MerchantAccountInformation[2].GlobalUniqueIdentifier);
            Assert.Equal("532631115550994", payload.MerchantAccountInformation[4].GlobalUniqueIdentifier);

            Assert.Equal("62003745", payload.AdditionalData.StoreLabel);
            Assert.Equal("62003745", payload.AdditionalData.TerminalLabel);
        }

        [Fact]
        public void DecodeQR4()
        {
            var qrData = "0002010102110213404587793527804155326311019494035204529553039365802GH5915JULITET LIMITED6005ACCRA62240708324313220308324313226304A5FA";
            var payload = MerchantPayload.FromQR(qrData);

            Assert.Equal(1, payload.PayloadFormatIndicator);
            Assert.Equal(11, payload.PointOfInitializationMethod);
            Assert.Equal("A5FA", payload.CRC);
            Assert.Equal(5295, payload.MerchantCategoryCode);
            Assert.Equal(936, payload.TransactionCurrency);
            Assert.Equal("JULITET LIMITED", payload.MerchantName);
            Assert.Equal("ACCRA", payload.MerchantCity);
            Assert.Equal("GH", payload.CountyCode);

            Assert.Equal("4045877935278", payload.MerchantAccountInformation[2].GlobalUniqueIdentifier);
            Assert.Equal("532631101949403", payload.MerchantAccountInformation[4].GlobalUniqueIdentifier);

            Assert.Equal("32431322", payload.AdditionalData.StoreLabel);
            Assert.Equal("32431322", payload.AdditionalData.TerminalLabel);
        }

        [Fact]
        public void DecodeQR5()
        {
            var qrData = "00020101021102154382871085619335204541153039365802GH5907PANDORA6005Accra63049C22";
            var payload = MerchantPayload.FromQR(qrData);

            Assert.Equal(1, payload.PayloadFormatIndicator);
            Assert.Equal(11, payload.PointOfInitializationMethod);
            Assert.Equal("9C22", payload.CRC);
            Assert.Equal(5411, payload.MerchantCategoryCode);
            Assert.Equal(936, payload.TransactionCurrency);
            Assert.Equal("PANDORA", payload.MerchantName);
            Assert.Equal("Accra", payload.MerchantCity);
            Assert.Equal("GH", payload.CountyCode);

            Assert.True(payload.MerchantAccountInformation.Count == 1);
            Assert.Equal("438287108561933", payload.MerchantAccountInformation[2].GlobalUniqueIdentifier);

            Assert.Null(payload.AdditionalData);
        }

        [Fact]
        public void PayloadWithSpecificationSample()
        {
            var merchantPayload = new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                PointOfInitializationMethod = 12,
                MerchantAccountInformation = new MerchantAccountInformationDictionary
                {
                    { 29, new MerchantAccountInformation
                        {
                            GlobalUniqueIdentifier = "D15600000000",
                            PaymentNetworkSpecific = new Dictionary<int, string>
                            {
                                { 5, "A93FO3230Q" }
                            }
                        }
                    },
                    { 31, new MerchantAccountInformation
                        {
                            GlobalUniqueIdentifier = "D15600000001",
                            PaymentNetworkSpecific = new Dictionary<int, string>
                            {
                                { 3, "12345678" }
                            }
                        }
                    }
                },
                MerchantCategoryCode = 4111,
                TransactionCurrency = Iso4217Currency.China.Value.NumericCode,
                TransactionAmount = 23.72m,
                CountyCode = Iso3166Countries.China,
                MerchantName = "BEST TRANSPORT",
                MerchantCity = "BEIJING",
                TipOrConvenienceIndicator = 1,
                MerchantInformation = new MerchantInfoLanguageTemplate
                {
                    LanguagePreference = "ZH",
                    MerchantNameAlternateLanguage = "最佳运输",
                    MerchantCityAlternateLanguage = "北京"
                },
                AdditionalData = new MerchantAdditionalData
                {
                    StoreLabel = "1234",
                    CustomerLabel = "***",
                    TerminalLabel = "A6008667",
                    AdditionalConsumerDataRequest = "ME"
                },
                UnreservedTemplate = new MerchantUnreservedDictionary
                {
                    {91, new MerchantUnreservedTemplate
                        {
                            GlobalUniqueIdentifier = "A011223344998877",
                            ContextSpecificData = new Dictionary<int, string>
                            {
                                {7, "12345678" }
                            }
                        }
                    }
                },
            };

            var payload = merchantPayload.GeneratePayload();
            Assert.Equal(
                expected: "00020101021229300012D156000000000510A93FO3230Q31280012D15600000001030812345678520441115802CN5914BEST TRANSPORT6007BEIJING64200002ZH0104最佳运输0202北京540523.7253031565502016233030412340603***0708A60086670902ME91320016A0112233449988770708123456786304A13A",
                actual: payload);
        }

        [Fact]
        public void PayloadWithMandatoryFields()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var merchantPayload = new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                MerchantAccountInformation = new MerchantAccountInformationDictionary
                {
                    {26, new MerchantAccountInformation { GlobalUniqueIdentifier = globalUniqueIdentifier} }
                },
                MerchantCategoryCode = 4111,
                TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                CountyCode = Iso3166Countries.Mexico,
                MerchantName = "My Super Shop",
                MerchantCity = "Mexico City",
            };

            var payload = merchantPayload.GeneratePayload();

            payload = AssertUtils.AssertThatContainsAndRemove(payload, "000201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"26360032{globalUniqueIdentifier}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "52044111");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5303{Iso4217Currency.MexicoPeso.Value.NumericCode}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5802{Iso3166Countries.Mexico}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5913My Super Shop");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6011Mexico City");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6304");
            Assert.True(4 == payload.Length);
        }

        [Fact]
        public void InvalidMerchantAccountInformationIdentifiers()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var merchantPayload = new MerchantPayload
                {
                    PayloadFormatIndicator = 1,
                    MerchantAccountInformation = new MerchantAccountInformationDictionary
                    {
                        {100, new MerchantAccountInformation { GlobalUniqueIdentifier = globalUniqueIdentifier} }
                    },
                    MerchantCategoryCode = 4111,
                    TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                    CountyCode = Iso3166Countries.Mexico,
                    MerchantName = "My Super Shop",
                    MerchantCity = "Mexico City",
                };

                merchantPayload.GeneratePayload();
            });
        }

        [Fact]
        public void InvalidMerchantAccountInformationPaymentSpecificItems()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var merchantPayload = new MerchantPayload
                {
                    PayloadFormatIndicator = 1,
                    MerchantAccountInformation = new MerchantAccountInformationDictionary
                    {
                        {26, new MerchantAccountInformation
                            {
                                GlobalUniqueIdentifier = globalUniqueIdentifier,
                                PaymentNetworkSpecific = new Dictionary<int, string>
                                {
                                    {150, "1234asdf" }
                                }
                            }
                        }
                    },
                    MerchantCategoryCode = 4111,
                    TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                    CountyCode = Iso3166Countries.Mexico,
                    MerchantName = "My Super Shop",
                    MerchantCity = "Mexico City",
                };

                merchantPayload.GeneratePayload();
            });
        }

        [Fact]
        public void InvalidPayloadFormatIndicator()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var merchantPayload = new MerchantPayload
                {
                    PayloadFormatIndicator = 10,
                    MerchantAccountInformation = new MerchantAccountInformationDictionary
                    {
                        {27, new MerchantAccountInformation { GlobalUniqueIdentifier = globalUniqueIdentifier} }
                    },
                    MerchantCategoryCode = 4111,
                    TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                    CountyCode = Iso3166Countries.Mexico,
                    MerchantName = "My Super Shop",
                    MerchantCity = "Mexico City",
                };

                merchantPayload.GeneratePayload();
            });
        }

        [Fact]
        public void InvalidTipOrConvenienceIndicator()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var merchantPayload = new MerchantPayload
                {
                    PayloadFormatIndicator = 1,
                    MerchantAccountInformation = new MerchantAccountInformationDictionary
                    {
                        {27, new MerchantAccountInformation { GlobalUniqueIdentifier = globalUniqueIdentifier} }
                    },
                    MerchantCategoryCode = 4111,
                    TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                    CountyCode = Iso3166Countries.Mexico,
                    MerchantName = "My Super Shop",
                    MerchantCity = "Mexico City",
                    TipOrConvenienceIndicator = 5
                };

                merchantPayload.GeneratePayload();
            });
        }

        [Fact]
        public void MissingFixedTip()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var merchantPayload = new MerchantPayload
                {
                    PayloadFormatIndicator = 1,
                    MerchantAccountInformation = new MerchantAccountInformationDictionary
                    {
                        {27, new MerchantAccountInformation { GlobalUniqueIdentifier = globalUniqueIdentifier} }
                    },
                    MerchantCategoryCode = 4111,
                    TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                    CountyCode = Iso3166Countries.Mexico,
                    MerchantName = "My Super Shop",
                    MerchantCity = "Mexico City",
                    TipOrConvenienceIndicator = 2
                };

                merchantPayload.GeneratePayload();
            });
        }

        [Fact]
        public void MissingPercentageTip()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var merchantPayload = new MerchantPayload
                {
                    PayloadFormatIndicator = 1,
                    MerchantAccountInformation = new MerchantAccountInformationDictionary
                    {
                        {27, new MerchantAccountInformation { GlobalUniqueIdentifier = globalUniqueIdentifier} }
                    },
                    MerchantCategoryCode = 4111,
                    TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                    CountyCode = Iso3166Countries.Mexico,
                    MerchantName = "My Super Shop",
                    MerchantCity = "Mexico City",
                    TipOrConvenienceIndicator = 3
                };

                merchantPayload.GeneratePayload();
            });
        }
    }
}
