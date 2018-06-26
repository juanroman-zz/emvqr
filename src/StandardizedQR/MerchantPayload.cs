using StandardizedQR.Services.Encoding;
using StandardizedQR.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace StandardizedQR
{
    public partial class MerchantPayload : IValidatableObject
    {
        private bool _validating;

        /// <summary>
        /// Defines the version of the QR Code template and hence the conventions on the identifiers, lengths, and values.
        /// </summary>
        /// <remarks>
        /// The Payload Format Indicator shall contain a value of "01". All other values are RFU.
        /// </remarks>
        [EmvSpecification(00, MaxLength = 2)]
        [Required]
        public int PayloadFormatIndicator { get; set; }

        /// <summary>
        /// Identifies the communication technology (here QR Code) and whether the data is static or dynamic
        /// </summary>
        /// <remarks>
        /// The Point of Initiation Method has a value of "11" for static QR Codes anda value of "12" for dynamic QR Codes.
        /// <para>The value of "11" is used when the same QR Code is shown for more than one transaction.</para>
        /// <para>The value of "12" is used when a new QR Code is shown for each transaction</para>
        /// </remarks>
        [EmvSpecification(01, MaxLength = 2)]
        [Range(11, 12)]
        public int? PointOfInitializationMethod { get; set; }

        /// <summary>
        /// Identifies the merchant.
        /// </summary>
        /// <remarks>
        /// The format and value are unique and specific to a payment system and several values may be included in the QR Code
        /// </remarks>
        [EmvSpecification(id: 29, IsParent = true)]
        [ValidateObject]
        public MerchantAccountInformationDictionary MerchantAccountInformation { get; set; }

        /// <summary>
        /// As defined by [ISO 18245] and assigned by the Acquirer
        /// </summary>
        [EmvSpecification(52, MaxLength = 4)]
        [Required]
        public int MerchantCategoryCode { get; set; }

        /// <summary>
        /// Indicates the currency code of the transaction. <see cref="Iso4217Currency"/>
        /// </summary>
        /// <remarks>
        /// A 3-digit numeric value, as defined by [ISO 4217]. This value will be used by the mobile application to display a recognizable currency to the consumer whenever an amount is being displayed or whenever the consumer is prompted to enter an amount.
        /// </remarks>
        [EmvSpecification(53, MaxLength = 3)]
        [Required]
        public int TransactionCurrency { get; set; }

        /// <summary>
        /// The transaction amount, if known. For instance, "99.34". If present, this value is displayed to the consumer by the mobile application when processing the transaction.If this data object is not present, the consumer is prompted to input the transaction amount to be paid to the merchant.
        /// </summary>
        [EmvSpecification(54, MaxLength = 13)]
        public decimal? TransactionAmount { get; set; }

        /// <summary>
        /// Indicates whether the consumer will be prompted to enter a tip or whether the merchant has determined that a flat, or percentage convenience fee is charged.
        /// </summary>
        [EmvSpecification(55, MaxLength = 2)]
        [Range(1, 3)]
        public int? TipOrConvenienceIndicator { get; set; }

        /// <summary>
        /// The fixed amount convenience fee when <see cref="TipOrConvenienceIndicator"/> indicates a flat convenience fee
        /// </summary>
        /// <example>
        /// For example, "9.85", indicating that this fixed amount (in the transaction currency) will be charged on top of the transaction amount.
        /// </example>
        [EmvSpecification(56, MaxLength = 13)]
        [MaxLength(13)]
        [RequireIso8859]
        public string ValueOfConvenienceFeeFixed { get; set; }

        /// <summary>
        /// The percentage convenience fee when <see cref="TipOrConvenienceIndicator"/> indicates a percentage convenience fee.
        /// </summary>
        /// <example>
        /// For example, "3.00" indicating that a convenience fee of 3% of the transaction amount will be charged, on top of the transaction amount.
        /// </example>
        [EmvSpecification(57, MaxLength = 5)]
        [RequireIso8859]
        public string ValueOfConvenienceFeePercentage { get; set; }

        /// <summary>
        /// Indicates the country of the merchant acceptance device. <see cref="Iso3166"/>
        /// </summary>
        /// <remarks>
        /// A 2-character alpha value, as defined by [ISO 3166-1 alpha 2] and assigned by the Acquirer.The country may be displayed to the consumer by the mobile application when processing the transaction
        /// </remarks>
        [EmvSpecification(58, MaxLength = 2)]
        [Required]
        [MaxLength(2)]
        [RequireIso8859]
        public string CountyCode { get; set; }

        /// <summary>
        /// The “doing business as” name for the merchant, recognizable to the consumer.This name may be displayed to the consumer by the mobile application when processing the transaction.
        /// </summary>
        [EmvSpecification(59)]
        [Required]
        [MaxLength(25)]
        [RequireIso8859]
        public string MerchantName { get; set; }

        /// <summary>
        /// City of operations for the merchant. This name may be displayed to the consumer by the mobile application when processing the transaction.
        /// </summary>
        [EmvSpecification(60, MaxLength = 15)]
        [Required]
        [MaxLength(15)]
        [RequireIso8859]
        public string MerchantCity { get; set; }

        /// <summary>
        /// Zip code or Pin code or Postal code of the merchant. If present, this value may be displayed to the consumer by the mobile application when processing the transaction.
        /// </summary>
        [EmvSpecification(61, MaxLength = 10)]
        [MaxLength(10)]
        [RequireIso8859]
        public string PostalCode { get; set; }

        [EmvSpecification(62, IsParent = true)]
        [ValidateObject]
        public MerchantAdditionalData AdditionalData { get; set; }

        /// <summary>
        /// Merchant Name and potentially other merchant related information in an alternate language, typically the local language.
        /// </summary>
        [EmvSpecification(64, IsParent = true)]
        [ValidateObject]
        public MerchantInfoLanguageTemplate MerchantInformation { get; set; }

        /// <summary>
        /// Gets or sets the unreserved template.
        /// </summary>
        /// <value>
        /// The unreserved template.
        /// </value>
        [EmvSpecification(91, IsParent = true)]
        [ValidateObject]
        public MerchantUnreservedDictionary UnreservedTemplate { get; set; }

        /// <summary>
        /// Checksum calculated over all the data objects included in the QR Code.
        /// </summary>
        [EmvSpecification(63, MaxLength = 4)]
        [MaxLength(4)]
        [RequireIso8859]
        public string CRC { get; internal set; }

        /// <summary>
        /// Generates the EMV(R) compliant payload that can be written into a QR code.
        /// </summary>
        /// <returns>Returns a <see cref="string"/> that contains the payload that can be written into a QR code.</returns>
        /// <exception cref="InvalidOperationException">If the payload fails validation.</exception>
        public string GeneratePayload()
        {
            var validationContext = new ValidationContext(this);
            var errors = Validate(validationContext);
            if (errors.Any())
            {
                var errorMessageBuilder = new StringBuilder();
                errorMessageBuilder.AppendLine(LibraryResources.ValidationError);

                foreach (var item in errors)
                {
                    errorMessageBuilder.AppendLine(item.ErrorMessage);
                }

                throw new InvalidOperationException(errorMessageBuilder.ToString());
            }

            var payload = new MerchantEncoder().GeneratePayload(this);
            return payload;
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (_validating)
            {
                return new ValidationResult[0];
            }

            try
            {
                _validating = true;

                var errors = new List<ValidationResult>();

                Validator.TryValidateObject(this, validationContext, errors, true);

                if (PayloadFormatIndicator != 1)
                {
                    errors.Add(new ValidationResult(LibraryResources.PayloadFormatIndicatorMustBe1, new string[] { nameof(PayloadFormatIndicator) }));
                }

                if (TipOrConvenienceIndicator.HasValue)
                {
                    switch (TipOrConvenienceIndicator.Value)
                    {
                        case 1:
                            // The mobile application should prompt the consumer to enter a tip to be paid to the merchant
                            break;

                        case 2:
                            if (string.IsNullOrWhiteSpace(ValueOfConvenienceFeeFixed))
                            {
                                errors.Add(new ValidationResult(LibraryResources.IfTipOrConvenienceIndicator2ThenDependencyRequired, new string[] { nameof(ValueOfConvenienceFeeFixed) }));
                            }
                            break;

                        case 3:
                            if (string.IsNullOrWhiteSpace(ValueOfConvenienceFeePercentage))
                            {
                                errors.Add(new ValidationResult(LibraryResources.IfTipOrConvenienceIndicator3ThenDependencyRequired, new string[] { nameof(ValueOfConvenienceFeePercentage) }));
                            }
                            break;

                        default:
                            errors.Add(new ValidationResult(LibraryResources.TipOrConvenienceIndicatorMustBeBetween1and3, new string[] { nameof(TipOrConvenienceIndicator) }));
                            break;
                    }
                }

                if (null != MerchantAccountInformation && 1 <= MerchantAccountInformation.Count)
                {
                    var invalidIdentifiers = MerchantAccountInformation.Keys.Count(k => k < 26 || k > 51);
                    if (0 < invalidIdentifiers)
                    {
                        errors.Add(new ValidationResult(LibraryResources.MerchantAccountInformationInvalidIdentifier, new string[] { nameof(MerchantAccountInformation) }));
                    }
                }
                else
                {
                    errors.Add(new ValidationResult(LibraryResources.MerchantAccountInformationIsRequired, new string[] { nameof(MerchantAccountInformation) }));
                }

                return errors;
            }
            finally
            {
                _validating = false;
            }
        }
    }
}
