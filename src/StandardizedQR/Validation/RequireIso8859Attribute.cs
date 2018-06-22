using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StandardizedQR.Validation
{
    public sealed class RequireIso8859Attribute : ValidationAttribute
    {
        private const string Iso8859EncodingName = "ISO-8859-1";

        public override bool IsValid(object value)
        {
            var str = value as string;
            if (!string.IsNullOrWhiteSpace(str))
            {
                var bytes = Encoding.GetEncoding(Iso8859EncodingName).GetBytes(str);
                var result = Encoding.GetEncoding(Iso8859EncodingName).GetString(bytes);

                return string.Equals(str, result);
            }

            return true;
        }
    }
}
