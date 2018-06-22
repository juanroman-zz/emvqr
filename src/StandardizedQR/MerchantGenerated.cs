using System;
using System.Collections.Generic;
using System.Text;

namespace StandardizedQR
{
    public class MerchantGenerated
    {
        /// <summary>
        /// Indicates whether a new QR Code is shown for each transaction <c>true</c> or if the same QR Code is shown for more than one transaction <c>false</c>.
        /// </summary>
        public bool UniquePerTransaction { get; set; }

        /// <summary>
        /// The transaction amount, if known. For instance, <c>99.34</c>. If present, this value is displayed to the consumer by the 
        /// mobile application when is prompted to input the transaction amount to be paid to the merchant.
        /// </summary>
        public decimal? TransactionAmount { get; set; }

        /// <summary>
        /// Indicates the currency code of the transaction. See <see cref="Iso4217Currency"/> for predefined codes.
        /// </summary>
        /// <remarks>
        /// A 3-digit numeric value, as defined by [ISO 4217]. This value will be used by the mobile application to display a recognizable currency to the consumer 
        /// whenever an amount is being displayed or whenever the consumer is prompted to enter an amount.
        /// </remarks>
        public int TransactionCurrency { get; set; }
    }
}
