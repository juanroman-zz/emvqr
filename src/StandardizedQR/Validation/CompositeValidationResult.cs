using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StandardizedQR.Validation
{
    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public CompositeValidationResult(string errorMessage) : base(errorMessage)
        {
        }

        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
        {
        }

        protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult)
        {
        }

        public IEnumerable<ValidationResult> Results => _results;

        public void AddResult(ValidationResult validationResult) => _results.Add(validationResult);
    }
}
