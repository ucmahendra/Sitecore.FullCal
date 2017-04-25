using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Sitecore.EventCalendar.Indexing
{
    public class EventIndex : SearchResultItem
    {
        [IndexField("eventtitle")]
        public string EventTitle { get; set; }

        [IndexField("eventsummary")]
        public string EventSummary { get; set; }

        [IndexField("eventshortsummary")]
        public string EventShortSummary { get; set; }

        [IndexField("eventdisplaydate")]
        public string DisplayDate { get; set; }

        [IndexField("eventlocation")]
        public string Location { get; set; }

        [IndexField("eventstartdate")]
        public DateTime EventStartDate { get; set; }

        [IndexField("eventenddate")]
        public DateTime EventEndDate { get; set; }

        [TypeConverter(typeof (IndexFieldIDValueConverter))]
        [IndexField("eventcategorytype")]
        [DataMember]
        public ID EventCategoryType { get; set; }

        [IndexField("eventimage")]
        public string EventImage { get; set; }

        [IndexField(BuiltinFields.LatestVersion)]
        public bool IsLatestVersion { get; set; }

        [TypeConverter(typeof (IndexFieldIDValueConverter))]
        [IndexField("eventcolor")]
        [DataMember]
        public ID EventClassName { get; set; }
    }
}