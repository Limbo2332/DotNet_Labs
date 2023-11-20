using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewsSite.BLL.Extensions
{
    public static class StringExtensions
    {
        public static bool ToDateTimeWithoutOutParameter(this string value)
        {
            return DateTime.TryParse(value, out _);
        }
    }
}
