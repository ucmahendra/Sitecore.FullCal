using System.Web.Script.Serialization;

namespace Sitecore.EventCalendar.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        ///     Extension to convert object to JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }
    }
}