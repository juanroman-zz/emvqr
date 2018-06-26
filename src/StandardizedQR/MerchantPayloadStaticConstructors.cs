namespace StandardizedQR
{
    public partial class MerchantPayload
    {
        public static MerchantPayload CreateStatic(string merchantGlobalUniqueIdentifier, int merchantCategoryCode, int transactionCurrencyNumericCode, string countryCode, string merchantName, string merchantCity)
        {
            return new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                PointOfInitializationMethod = 11,
                MerchantCategoryCode = merchantCategoryCode,
                TransactionCurrency = transactionCurrencyNumericCode,
                CountyCode = countryCode,
                MerchantName = merchantName,
                MerchantCity = merchantCity,
                MerchantAccountInformation = new MerchantAccountInformationDictionary
                {
                    {26, new MerchantAccountInformation { GlobalUniqueIdentifier = merchantGlobalUniqueIdentifier} }
                }
            };
        }

        public static MerchantPayload CreateDynamic(string merchantGlobalUniqueIdentifier, int merchantCategoryCode, int transactionCurrencyNumericCode, string countryCode, string merchantName, string merchantCity)
        {
            return new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                PointOfInitializationMethod = 12,
                MerchantCategoryCode = merchantCategoryCode,
                TransactionCurrency = transactionCurrencyNumericCode,
                CountyCode = countryCode,
                MerchantName = merchantName,
                MerchantCity = merchantCity,
                MerchantAccountInformation = new MerchantAccountInformationDictionary
                {
                    {26, new MerchantAccountInformation { GlobalUniqueIdentifier = merchantGlobalUniqueIdentifier} }
                }
            };
        }
    }
}
