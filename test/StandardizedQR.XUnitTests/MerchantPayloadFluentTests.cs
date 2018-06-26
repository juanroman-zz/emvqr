using System;
using Xunit;

namespace StandardizedQR.XUnitTests
{
    public class MerchantPayloadFluentTests
    {
        [Fact]
        public void StaticMandatoryPayload()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var merchantPayload = MerchantPayload.CreateStatic(
                globalUniqueIdentifier,
                4111,
                Iso4217Currency.MexicoPeso.Value.NumericCode,
                Iso3166Countries.Mexico,
                "My Super Shop",
                "Mexico City");

            var payload = merchantPayload.GeneratePayload();

            payload = AssertUtils.AssertThatContainsAndRemove(payload, "000201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "010211"); // static
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
        public void DynamicMandatoryPayload()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var merchantPayload = MerchantPayload.CreateDynamic(
                globalUniqueIdentifier,
                4111,
                Iso4217Currency.MexicoPeso.Value.NumericCode,
                Iso3166Countries.Mexico,
                "My Super Shop",
                "Mexico City");

            var payload = merchantPayload.GeneratePayload();

            payload = AssertUtils.AssertThatContainsAndRemove(payload, "000201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "010212"); // dynamic
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
        public void DynamicWithTransactionAmount()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var merchantPayload = MerchantPayload.CreateDynamic(
                globalUniqueIdentifier,
                4111,
                Iso4217Currency.MexicoPeso.Value.NumericCode,
                Iso3166Countries.Mexico,
                "My Super Shop",
                "Mexico City")
                .WithTransactionAmount(100);

            var payload = merchantPayload.GeneratePayload();

            payload = AssertUtils.AssertThatContainsAndRemove(payload, "000201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "010212"); // dynamic
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"26360032{globalUniqueIdentifier}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "52044111");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5303{Iso4217Currency.MexicoPeso.Value.NumericCode}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5802{Iso3166Countries.Mexico}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5913My Super Shop");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6011Mexico City");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5406100.00");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6304");
            Assert.True(4 == payload.Length);
        }

        [Fact]
        public void DynamicWithTipIndicatedByUser()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var merchantPayload = MerchantPayload.CreateDynamic(
                globalUniqueIdentifier,
                4111,
                Iso4217Currency.MexicoPeso.Value.NumericCode,
                Iso3166Countries.Mexico,
                "My Super Shop",
                "Mexico City")
                .WithTransactionAmount(100)
                .WithTipByUser();

            var payload = merchantPayload.GeneratePayload();

            payload = AssertUtils.AssertThatContainsAndRemove(payload, "000201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "010212"); // dynamic
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"26360032{globalUniqueIdentifier}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "52044111");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5303{Iso4217Currency.MexicoPeso.Value.NumericCode}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5802{Iso3166Countries.Mexico}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5913My Super Shop");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6011Mexico City");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5406100.00");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "550201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6304");
            Assert.True(4 == payload.Length);
        }

        [Fact]
        public void DynamicWithFixedTip()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var merchantPayload = MerchantPayload.CreateDynamic(
                globalUniqueIdentifier,
                4111,
                Iso4217Currency.MexicoPeso.Value.NumericCode,
                Iso3166Countries.Mexico,
                "My Super Shop",
                "Mexico City")
                .WithTransactionAmount(100)
                .WithTipFixed(15);

            var payload = merchantPayload.GeneratePayload();

            payload = AssertUtils.AssertThatContainsAndRemove(payload, "000201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "010212"); // dynamic
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"26360032{globalUniqueIdentifier}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "52044111");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5303{Iso4217Currency.MexicoPeso.Value.NumericCode}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5802{Iso3166Countries.Mexico}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5913My Super Shop");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6011Mexico City");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5406100.00");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "550202");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "560515.00");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6304");
            Assert.True(4 == payload.Length);
        }

        [Fact]
        public void DynamicWithTipPercentage()
        {
            var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

            var merchantPayload = MerchantPayload.CreateDynamic(
                globalUniqueIdentifier,
                4111,
                Iso4217Currency.MexicoPeso.Value.NumericCode,
                Iso3166Countries.Mexico,
                "My Super Shop",
                "Mexico City")
                .WithTransactionAmount(100)
                .WithTipPercentage(0.15d);

            var payload = merchantPayload.GeneratePayload();

            payload = AssertUtils.AssertThatContainsAndRemove(payload, "000201");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "010212"); // dynamic
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"26360032{globalUniqueIdentifier}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "52044111");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5303{Iso4217Currency.MexicoPeso.Value.NumericCode}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, $"5802{Iso3166Countries.Mexico}");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5913My Super Shop");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6011Mexico City");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "5406100.00");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "550203");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "570315%");
            payload = AssertUtils.AssertThatContainsAndRemove(payload, "6304");
            Assert.True(4 == payload.Length);
        }
    }
}
