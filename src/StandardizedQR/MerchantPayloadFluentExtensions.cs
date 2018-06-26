namespace StandardizedQR
{
    public static class MerchantPayloadFluentExtensions
    {
        public static MerchantPayload WithTransactionAmount(this MerchantPayload merchantPayload, decimal transactionAmount)
        {
            merchantPayload.TransactionAmount = transactionAmount;

            return merchantPayload;
        }

        public static MerchantPayload WithTipByUser(this MerchantPayload merchantPayload)
        {
            merchantPayload.TipOrConvenienceIndicator = 1;

            return merchantPayload;
        }

        public static MerchantPayload WithTipFixed(this MerchantPayload merchantPayload, decimal fixedTip)
        {
            merchantPayload.TipOrConvenienceIndicator = 2;
            merchantPayload.ValueOfConvenienceFeeFixed = fixedTip.ToString("#.00");

            return merchantPayload;
        }

        public static MerchantPayload WithTipPercentage(this MerchantPayload merchantPayload, double tipPercentage)
        {
            merchantPayload.TipOrConvenienceIndicator = 3;
            merchantPayload.ValueOfConvenienceFeePercentage = tipPercentage.ToString("P0").Replace(" ", string.Empty);

            return merchantPayload;
        }

        public static MerchantPayload WithAdditionalData(this MerchantPayload merchantPayload,
            string billNumber = null,
            string mobileNumber = null,
            string storeLabel = null,
            string loyaltyNumber = null,
            string referenceLabel = null,
            string terminalLabel = null,
            string purposeOfTransaction = null,
            string additionalConsumerDataRequest = null,
            string customerLabel = null)
        {
            merchantPayload.AdditionalData = new MerchantAdditionalData
            {
                AdditionalConsumerDataRequest = additionalConsumerDataRequest,
                BillNumber = billNumber,
                CustomerLabel = customerLabel,
                LoyaltyNumber = loyaltyNumber,
                MobileNumber = mobileNumber,
                PurposeOfTransaction = purposeOfTransaction,
                ReferenceLabel = referenceLabel,
                StoreLabel = storeLabel,
                TerminalLabel = terminalLabel
            };

            return merchantPayload;
        }


        public static MerchantPayload WithAlternateLanguage(this MerchantPayload merchantPayload, string languagePreference, string merchantName, string merchantCity = null)
        {
            merchantPayload.MerchantInformation = new MerchantInfoLanguageTemplate
            {
                LanguagePreference = languagePreference,
                MerchantCityAlternateLanguage = merchantCity,
                MerchantNameAlternateLanguage = merchantName
            };

            return merchantPayload;
        }
    }
}
