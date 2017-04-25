using System;
using Sitecore.Data;

namespace Sitecore.EventCalendar.EventModels
{
    public class EventParam
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CategoryList { get; set; }
        public string Query { get; set; }
        public string ItemId { get; set; }
    }
}