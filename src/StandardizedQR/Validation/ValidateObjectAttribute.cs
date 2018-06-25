using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace StandardizedQR.Validation
{
    /// <summary>
    /// Helper attribute that allows for recursive valdation using data annotations.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class ValidateObjectAttribute : ValidationAttribute
    {
        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResult"></see> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (null != value)
            {
                var context = new ValidationContext(value, null, null);
                Validator.TryValidateObject(value, context, results, true);

                if (0 != results.Count)
                {
                    var compositeResults = new CompositeValidationResult(string.Format(CultureInfo.CurrentCulture, LibraryResources.CompositeValidationFailed, validationContext.DisplayName));
                    results.ForEach(compositeResults.AddResult);

                    return compositeResults;
                }
            }

            return ValidationResult.Success;
        }
    }
}
