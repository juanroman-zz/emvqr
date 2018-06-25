using StandardizedQR.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StandardizedQR
{
    /// <summary>
    /// For Unreserved Templates with IDs "80" to "99", the primitive data object 'Globally Unique Identifier' with ID "00" 
    /// shall be included in the template. Its value sets the context for the remainder of the template and the meaning of the 
    /// other IDs and data objects in the template are context specific and outside of the scope of EMVCo.
    /// </summary>
    /// <remarks>
    /// <para>Zero or more Unreserved Templates may be present.</para>
    /// <para>Unreserved Templates can be allocated and used by other parties, such as (domestic) payment systems and 
    /// value-added service providers, for their own products.They can then define the meaning, representation and format.
    /// Each payment system provider or value-added service provider puts their data in a separate Unreserved Template ID.
    /// For example, the first set of data is placed in ID “80”, the second set of data is placed in ID “81”, and so on.</para>
    /// </remarks>
    public class MerchantUnreservedTemplate
    {
        private bool _validating;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantUnreservedTemplate"/> class.
        /// </summary>
        public MerchantUnreservedTemplate()
        {
            ContextSpecificData = new Dictionary<int, string>();
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
        public Dictionary<int, string> ContextSpecificData { get; set; }

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


                var invalidIdentifiers = ContextSpecificData.Keys.Count(k => k < 1 || k > 99);
                if (0 < invalidIdentifiers)
                {
                    validationResults.Add(new ValidationResult("ContextSpecificData", new string[] { nameof(ContextSpecificData) }));
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
