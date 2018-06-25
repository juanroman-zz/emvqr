using StandardizedQR.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StandardizedQR
{
    /// <summary>
    /// Defines attributes for the Merchant Account Information template.
    /// </summary>
    /// <seealso cref="IValidatableObject" />
    public class MerchantAccountInformation : IValidatableObject
    {
        private bool _validating;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantAccountInformation"/> class.
        /// </summary>
        public MerchantAccountInformation()
        {
            PaymentNetworkSpecific = new Dictionary<int, string>();
        }

        /// <summary>
        /// An identifier that sets the context of the data that follows. The value is one of the following:
        /// <para>an Application Identifier (AID)</para>
        /// <para>a [UUID] without the hyphen (-) separators</para>
        /// <para>a reverse domain name</para>
        /// </summary>
        [EmvSpecification(0, MaxLength = 32)]
        [Required]
        [RequireIso8859]
        [MaxLength(32)]
        public string GlobalUniqueIdentifier { get; set; }

        /// <summary>
        /// Association of data objects to IDs and type of data object is specific to the Globally Unique Identifier.
        /// </summary>
        /// <remarks>
        /// Identifiers must be between <c>1</c> and <c>99</c>.
        /// </remarks>
        public Dictionary<int, string> PaymentNetworkSpecific { get; set; }

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


                var invalidIdentifiers = PaymentNetworkSpecific.Keys.Count(k => k < 1 || k > 99);
                if (0 < invalidIdentifiers)
                {
                    validationResults.Add(new ValidationResult("PaymentNetworkSpecific", new string[] { nameof(PaymentNetworkSpecific) }));
                }

                return validationResults;
            }
            finally
            {
                _validating = false;
            }
        }
    }
}
