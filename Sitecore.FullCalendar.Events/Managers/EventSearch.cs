using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.EventCalendar.EventModels;
using Sitecore.EventCalendar.Extensions;
using Sitecore.EventCalendar.Indexing;
using Sitecore.Links;

namespace Sitecore.EventCalendar.Managers
{
    public class EventSearch
    {
        /// <summary>
        ///     Get event items from lucene search
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static List<EventProperties> GetEvents(EventParam request)
        {
            var searchIndex = SearchConstants.Index;
            using (var context = searchIndex.CreateSearchContext())
            {
                var currentMonth = DateTime.Today.ToUniversalTime().Month;
                // Get base query
                var matches = GetBaseQuery(context);
                if (!request.StartDate.IsNullOrEmpty() && !request.EndDate.IsNullOrEmpty())
                {
                    var startDateFormat = request.StartDate.Replace(' ', '+');
                    var startDate = DateTime.Parse(startDateFormat);

                    var endDateFormat = request.EndDate.Replace(' ', '+');
                    var endDate = DateTime.Parse(endDateFormat);
                    matches =
                        matches.Filter(
                            x =>
                                (x.EventStartDate > startDate && x.EventStartDate < endDate) ||
                                (x.EventStartDate < startDate && x.EventEndDate > startDate));
                }
                else
                {
                    matches = matches.Filter(x => x.EventStartDate.Month == currentMonth);
                }

                // predicate for event category
                if (request.CategoryList != null && request.CategoryList.Any() && !request.CategoryList.IsNullOrEmpty())
                {
                    var categList = request.CategoryList.Split(',').Select(ID.Parse).ToList();
                    if (categList.Any())
                    {
                        matches = matches.Where(FilterByCatIds(categList));
                    }
                }

                // predicate for search query
                if (!string.IsNullOrEmpty(request.Query))
                {
                    var searchpredicate = PredicateBuilder.True<EventIndex>();
                    searchpredicate = request.Query.Split(' ')
                        .Aggregate(searchpredicate, (current, s) => current.Or(p => p.Content.Contains(s)));
                    matches = matches.Where(searchpredicate);
                }

                return matches.GetResults().Hits.Select(hit => new EventProperties
                {
                    title = hit.Document.EventTitle,
                    eventShortSummary = hit.Document.EventShortSummary,
                    location = hit.Document.Location,
                    start = hit.Document.EventStartDate.ToString("s"),
                    end = hit.Document.EventEndDate.ToString("s"),
                    eventCategoryType = hit.Document.EventCategoryType,
                    className = EventClassName(hit.Document.EventCategoryType),
                    color = EventBGColor(hit.Document.EventCategoryType),
                    eventItemId = hit.Document.ItemId,
                    url = LinkManager.GetItemUrl(hit.Document.GetItem())
                }).ToList();
            }
        }

        /// <summary>
        ///     Get event class name
        /// </summary>
        /// <param name="eventCategoryId"></param>
        /// <returns></returns>
        public static string EventClassName(ID eventCategoryId)
        {
            var eventClassName = "";
            if (ID.IsNullOrEmpty(eventCategoryId)) return eventClassName;
            var eventCatItem = Context.Database.GetItem(eventCategoryId);
            if (eventCatItem != null)
            {
                var eventColorItem = ((LookupField)eventCatItem.Fields["Event Color"]).TargetItem;
                if (eventColorItem != null)
                {
                    eventClassName = eventColorItem.Fields["Meta Value"].Value;
                }
            }
            return eventClassName;
        }

        /// <summary>
        ///     Get event background color
        /// </summary>
        /// <param name="eventCategoryId"></param>
        /// <returns></returns>
        public static string EventBGColor(ID eventCategoryId)
        {
            var eventClassName = "";
            if (ID.IsNullOrEmpty(eventCategoryId)) return eventClassName;
            var eventCatItem = Context.Database.GetItem(eventCategoryId);
            if (eventCatItem != null)
            {
                eventClassName = eventCatItem.Fields["Event BG Color"].Value;
            }
            return eventClassName;
        }

        /// <summary>
        ///     Event filter predicate
        /// </summary>
        /// <param name="categList"></param>
        /// <returns></returns>
        public static Expression<Func<EventIndex, bool>> FilterByCatIds(IEnumerable<ID> categList)
        {
            var predicate = PredicateBuilder.False<EventIndex>();

            foreach (var catList in categList)
            {
                ID id;
                if (ID.TryParse(catList, out id))
                {
                    predicate = predicate.Or(x => x.EventCategoryType == id);
                }
            }
            return predicate;
        }

        /// <summary>
        ///     Find by item name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static EventIndex FindByItemName(string name)
        {
            var index = SearchConstants.Index;

            using (var context = index.CreateSearchContext())
            {
                var matches = GetBaseQuery(context).Filter(x => x.Name == name);
                return matches.Take(1).ToList().FirstOrDefault(i => i != null);
            }
        }

        /// <summary>
        ///     Get base query
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<EventIndex> GetBaseQuery(IProviderSearchContext context)
        {
            string eventRootItem = null;
            var settingItem = Sitecore.Context.Database.GetItem(EventConfigSettings.EventCalendarSettingItem);
            if (settingItem != null)
            {
                if (settingItem.Fields["Event Calendar Root"] != null && !settingItem.Fields["Event Calendar Root"].Value.IsNullOrEmpty())
                {
                    eventRootItem = Sitecore.Context.Database.GetItem(settingItem.Fields["Event Calendar Root"].Value).ID.ToString();
                }
                else
                {
                    eventRootItem = EventConfigSettings.EventCalendarRootID;
                }
            }
            return context.GetQueryable<EventIndex>(new CultureExecutionContext(Context.Culture))
                .Filter(x => x.Language == Context.Language.Name
                             && x.IsLatestVersion
                             && x.TemplateId == ID.Parse(EventConfigSettings.EventTemplateID)
                             && x.Paths.Contains(ID.Parse(eventRootItem)));
        }

        /// <summary>
        ///     Get event items
        /// </summary>
        /// <returns></returns>
        public static List<Item> GetEvents()
        {
            var index = SearchConstants.Index;

            using (var context = index.CreateSearchContext())
            {
                var matches = GetBaseQuery(context);

                return matches.ToList().Select(i => Context.Database.GetItem(i.ItemId)).Where(i => i != null).ToList();
            }
        }
    }

    public class EventProperties
    {
        public string eventShortSummary { get; set; }
        public string location { get; set; }
        public ID eventCategoryType { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string className { get; set; }
        public string color { get; set; }
        public ID eventItemId { get; set; }
        public string url { get; set; }
    }
}