using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StandardizedQR.UnitTests
{
    [TestClass]
    public class MerchantPayloadUnitTests
    {
        [TestMethod]
        public void PayloadWithRequiredFields()
        {
            var merchantPayload = new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                PointOfInitializationMethod = 12,
                MerchantAccountInformation = Guid.NewGuid().ToString(),
                MerchantCategoryCode = 4111,
                TransactionCurrency = Iso4217Currency.China.Value.NumericCode,
                TransactionAmount = 23.72m,
                CountyCode = Iso3166Countries.China,
                MerchantName = "Best Transport",
                MerchantCity = "Beijing",
                TipOrConvenienceIndicator = 1,
                CRC = "null"
            };

            var payload = merchantPayload.GeneratePayload();
            Assert.IsNotNull(payload);
        }

        [TestMethod]
        public void RequiredFields()
        {
            var merchantPayload = new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                MerchantAccountInformation = Guid.NewGuid().ToString(),
                MerchantCategoryCode = 4941,
                TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                CountyCode = Iso3166Countries.Mexico,
                MerchantName = "My Super Shop",
                MerchantCity = "Mexico City",
                CRC = "null"
            };

            var validationContext = new ValidationContext(merchantPayload);
            var errors = merchantPayload.Validate(validationContext);
            var errorList = new List<ValidationResult>(errors);
            Assert.AreEqual(0, errorList.Count);
        }

        [TestMethod]
        public void RequiredAndPercentageTip()
        {
            var merchantPayload = new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                MerchantAccountInformation = Guid.NewGuid().ToString(),
                MerchantCategoryCode = 4941,
                TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                CountyCode = Iso3166Countries.Mexico,
                MerchantName = "My Super Shop",
                MerchantCity = "Mexico City",
                CRC = "null",
                TransactionAmount = 85m,
                TipOrConvenienceIndicator = 3,
                ValueOfConvenienceFeePercentage = "15",
            };

            var validationContext = new ValidationContext(merchantPayload);
            var errors = merchantPayload.Validate(validationContext);
            var errorList = new List<ValidationResult>(errors);
            Assert.AreEqual(0, errorList.Count);
        }

        [TestMethod]
        public void RequiredAndFixedTip()
        {
            var merchantPayload = new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                MerchantAccountInformation = Guid.NewGuid().ToString(),
                MerchantCategoryCode = 4941,
                TransactionCurrency = Iso4217Currency.MexicoPeso.Value.NumericCode,
                CountyCode = Iso3166Countries.Mexico,
                MerchantName = "My Super Shop",
                MerchantCity = "Mexico City",
                CRC = "null",
                TransactionAmount = 85.45m,
                TipOrConvenienceIndicator = 2,
                ValueOfConvenienceFeeFixed = "15",
            };

            var validationContext = new ValidationContext(merchantPayload);
            var errors = merchantPayload.Validate(validationContext);
            var errorList = new List<ValidationResult>(errors);
            Assert.AreEqual(0, errorList.Count);
        }
    }
}
