namespace Sitecore.EventCalendar.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Indicates whether the specified string is null or an empty string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}