using StandardizedQR.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StandardizedQR
{
    /// <summary>
    /// If this template is present, it shall contain the Language Preference (ID "00") and Merchant Name—Alternate Language (ID "01"). It may contain the Merchant City—Alternate Language(ID "02").
    /// </summary>
    public class MerchantInfoLanguageTemplate : IValidatableObject
    {
        private bool _validating;

        /// <summary>
        /// The language Preference shall contain 2 alphabetical characters coded to a value defined by [ISO 639].
        /// </summary>
        /// <value>
        /// The language preference.
        /// </value>
        /// <remarks>
        /// The value should represent the single language used to encode the Merchant Name—Alternate Language and the optional Merchant City—Alternate Language.
        /// </remarks>
        [EmvSpecification(0, MaxLength = 2)]
        [RequireIso8859]
        [MaxLength(2)]
        [Required]
        public string LanguagePreference { get; set; }

        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        /// <value>
        /// The name of the merchant.
        /// </value>
        /// <remarks>
        /// The Merchant Name—Alternate Language should indicate the “doing business as” name for the merchant in the merchant’s local language.
        /// </remarks>
        [EmvSpecification(1)]
        [Required]
        [MaxLength(25)]
        public string MerchantNameAlternateLanguage { get; set; }

        /// <summary>
        /// If present, the Merchant City—Alternate Language should indicate the city in which the merchant transacts in the merchant’s local language.
        /// </summary>
        /// <value>
        /// The merchant city.
        /// </value>
        [EmvSpecification(2)]
        [MaxLength(15)]
        public string MerchantCityAlternateLanguage { get; set; }

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

                var validationResults = new List<ValidationResult>();

                Validator.TryValidateObject(this, validationContext, validationResults);

                return validationResults;
            }
            finally
            {
                _validating = false;
            }
        }
    }
}
