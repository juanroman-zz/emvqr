using System.Reflection;
using System.Text;

namespace StandardizedQR.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Converts bytes to its Hexadecimal representation
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array.</param>
        /// <param name="upperCase">if set to <c>true</c> returns a string in upper case.</param>
        /// <returns>
        /// Returns the hexadecimal representation of each byte in an array.
        /// </returns>
        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            var sb = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the last X characters from a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="tailLength">The number of characters to get.</param>
        /// <returns>Returns the last X characters from a string.</returns>
        public static string GetLast(this string input, int tailLength)
        {
            if (tailLength >= input.Length)
            {
                return input;
            }

            return input.Substring(input.Length - tailLength);
        }

        public static void SetAndCastValue(this PropertyInfo property, object obj, string value)
        {
            if (property.PropertyType.IsAssignableFrom(typeof(int)))
            {
                property.SetValue(obj, int.Parse(value));
            }
            else if (property.PropertyType.IsAssignableFrom(typeof(decimal)))
            {
                property.SetValue(obj, decimal.Parse(value));
            }
            else
            {
                property.SetValue(obj, value);
            }
        }
    }
}
