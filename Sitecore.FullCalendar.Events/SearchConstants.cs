using Sitecore.ContentSearch;

namespace Sitecore.EventCalendar
{
    public class SearchConstants
    {
        private static ISearchIndex _index;
        // Get Indexing
        public static string IndexName
        {
            get { return "eventcalendar_index"; }
        }

        public static ISearchIndex Index
        {
            get { return _index ?? (_index = ContentSearchManager.GetIndex(IndexName)); }
        }
    }
}