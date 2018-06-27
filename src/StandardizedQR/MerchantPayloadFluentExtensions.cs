using System;
using System.Collections.Generic;

namespace StandardizedQR
{
    public static class MerchantPayloadFluentExtensions
    {
        /// <summary>
        /// Indicates if the client application should the prompt for a bill number.
        /// </summary>
        public static bool ShouldPromptForBillNumber(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.BillNumber);

        /// <summary>
        /// Indicates if the client application should the prompt for a mobile number.
        /// </summary>
        public static bool ShouldPromptForMobileNumber(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.MobileNumber);

        /// <summary>
        /// Indicates if the client application should the prompt for a store label.
        /// </summary>
        public static bool ShouldPromptForStoreLabel(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.StoreLabel);

        /// <summary>
        /// Indicates if the client application should the prompt for a loyalty number.
        /// </summary>
        public static bool ShouldPromptForLoyaltyNumber(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.LoyaltyNumber);

        /// <summary>
        /// Indicates if the client application should the prompt for a reference label.
        /// </summary>
        public static bool ShouldPromptForReferenceLabel(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.ReferenceLabel);

        /// <summary>
        /// Indicates if the client application should the prompt for a customer label.
        /// </summary>
        public static bool ShouldPromptForCustomerLabel(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.CustomerLabel);

        /// <summary>
        /// Indicates if the client application should the prompt for the terminal label.
        /// </summary>
        public static bool ShouldPromptForTerminalLabel(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.TerminalLabel);

        /// <summary>
        /// Indicates if the client application should automaticaly add the consumer address to the transaction.
        /// </summary>
        public static bool ShouldAddConsumerAddress(this MerchantPayload merchantPayload) =>
            null != merchantPayload.AdditionalData && merchantPayload.AdditionalData.AdditionalConsumerDataRequest.Contains("A");

        /// <summary>
        /// Indicates if the client application should automaticaly add the consumer mobile number to the transaction.
        /// </summary>
        public static bool ShouldAddConsumerMobileNumber(this MerchantPayload merchantPayload) =>
            null != merchantPayload.AdditionalData && merchantPayload.AdditionalData.AdditionalConsumerDataRequest.Contains("M");

        /// <summary>
        /// Indicates if the client application should automaticaly add the consumer email to the transaction.
        /// </summary>
        public static bool ShouldAddConsumerEmail(this MerchantPayload merchantPayload) =>
            null != merchantPayload.AdditionalData && merchantPayload.AdditionalData.AdditionalConsumerDataRequest.Contains("E");

        /// <summary>
        /// Indicates if the client application should the prompt for purpose of the transaction.
        /// </summary>
        public static bool ShouldPromptForPurposeOfTransaction(this MerchantPayload merchantPayload) =>
            0 == StringComparer.InvariantCultureIgnoreCase.Compare("***", merchantPayload.AdditionalData?.PurposeOfTransaction);

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

        public static MerchantPayload WithUnreservedTemplate(this MerchantPayload merchantPayload, string globalUniqueIdentifier, Dictionary<int, string> contextSpecificData)
        {
            return merchantPayload.WithUnreservedTemplate(80, globalUniqueIdentifier, contextSpecificData);
        }

        public static MerchantPayload WithUnreservedTemplate(this MerchantPayload merchantPayload, int id, string globalUniqueIdentifier, Dictionary<int, string> contextSpecificData)
        {
            merchantPayload.UnreservedTemplate = new MerchantUnreservedDictionary
            {
                { 80, new MerchantUnreservedTemplate { GlobalUniqueIdentifier = globalUniqueIdentifier, ContextSpecificData = contextSpecificData} }
            };

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
