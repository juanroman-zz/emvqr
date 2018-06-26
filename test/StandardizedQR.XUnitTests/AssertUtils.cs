using Xunit;

namespace StandardizedQR.XUnitTests
{
    internal static class AssertUtils
    {
        public static string AssertThatContainsAndRemove(string actual, string expected)
        {
            Assert.Contains(expected, actual);
            return actual.Replace(expected, string.Empty);
        }
    }
}
