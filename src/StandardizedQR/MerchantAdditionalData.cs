using StandardizedQR.Validation;
using System.ComponentModel.DataAnnotations;

namespace StandardizedQR
{
    /// <summary>
    /// The Additional Data Field Template includes information that may be provided by the Merchant or may be populated by 
    /// the mobile application to enable or facilitate certain use cases.
    /// </summary>
    public class MerchantAdditionalData
    {
        /// <summary>
        /// Gets or sets the bill number.
        /// </summary>
        /// <value>
        /// The invoice number or bill number. This number could be provided by the merchant or could be an indication for 
        /// the mobile application to prompt the consumer to input a Bill Number.
        /// </value>
        [EmvSpecification(1, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string BillNumber { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        /// <value>
        /// The mobile number could be provided by the merchant or could be an indication for the mobile application to
        /// prompt the consumer to input a Mobile Number.
        /// </value>
        [EmvSpecification(2, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the store label.
        /// </summary>
        /// <value>
        /// A distinctive value associated to a store. This value could be provided by the merchant or could be an indication 
        /// for the mobile application to prompt the consumer to input a Store Label.
        /// </value>
        [EmvSpecification(3, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string StoreLabel { get; set; }

        /// <summary>
        /// Gets or sets the loyalty number.
        /// </summary>
        /// <value>
        /// Typically, a loyalty card number. This number could be provided by the merchant, if known, or could be an indication 
        /// for the mobile application to prompt the consumer to input their Loyalty Number.
        /// </value>
        [EmvSpecification(4, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string LoyaltyNumber { get; set; }

        /// <summary>
        /// Gets or sets the reference label.
        /// </summary>
        /// <value>
        /// Any value as defined by the merchant or acquirer in order to identify the transaction. This value could be provided 
        /// by the merchant or could be an indication for the mobile app to prompt the consumer to input a transaction Reference Label.
        /// </value>
        [EmvSpecification(5, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string ReferenceLabel { get; set; }

        /// <summary>
        /// Gets or sets the customer label.
        /// </summary>
        /// <value>
        /// Any value identifying a specific consumer. This value could be provided by the merchant (if known), or could be an 
        /// indication for the mobile application to prompt the consumer to input their Customer Label.
        /// </value>
        [EmvSpecification(6, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string CustomerLabel { get; set; }

        /// <summary>
        /// Gets or sets the terminal label.
        /// </summary>
        /// <value>
        /// A distinctive value associated to a terminal in the store. This value could be provided by the merchant or could be 
        /// an indication for the mobile application to prompt the consumer to input a Terminal Label.
        /// </value>
        [EmvSpecification(7, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string TerminalLabel { get; set; }

        /// <summary>
        /// Gets or sets the purpose of transaction.
        /// </summary>
        /// <value>
        /// Any value defining the purpose of the transaction. This value could be provided by the merchant or could be an 
        /// indication for the mobile application to prompt the consumer to input a value describing the purpose of the transaction.
        /// </value>
        [EmvSpecification(8, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string PurposeOfTransaction { get; set; }

        /// <summary>
        /// Gets or sets the additional consumer data request.
        /// </summary>
        /// <value>
        /// Contains indications that the mobile application is to provide the requested information in order to complete the 
        /// transaction. The information requested should be provided by the mobile application in the authorization without 
        /// unnecessarily prompting the consumer.
        /// </value>
        [EmvSpecification(9, MaxLength = 25)]
        [RequireIso8859]
        [MaxLength(25)]
        public string AdditionalConsumerDataRequest { get; set; }
    }
}
