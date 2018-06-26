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
            merchantPayload.ValueOfConvenienceFeePercentage = tipPercentage.ToString("P0");

            return merchantPayload;
        }
    }
}
