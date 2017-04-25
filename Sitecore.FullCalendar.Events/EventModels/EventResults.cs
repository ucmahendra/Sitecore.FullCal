using System.Collections.Generic;
using Sitecore.EventCalendar.Managers;

namespace Sitecore.EventCalendar.EventModels
{
    public class EventResults
    {
        public IEnumerable<EventProperties> EventViewResult { get; set; }
    }
}