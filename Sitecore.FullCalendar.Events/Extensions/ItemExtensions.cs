using System.Collections.Generic;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;

namespace Sitecore.EventCalendar.Extensions
{
    public static class ItemExtensions
    {
        /// <summary>
        ///     Gets the Template of an item (which is different than the TemplateItem)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Template GetTemplate(this Item item)
        {
            return TemplateManager.GetTemplate(item);
        }

        /// <summary>
        ///     Check if the item's template inherites from the given template
        /// </summary>
        /// <param name="item"></param>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static bool InheritsTemplate(this Item item, string templateID)
        {
            return item.GetTemplate().InheritsFrom(new ID(templateID));
        }
    }

    public static class CalendarItemExtensions
    {
        /// <summary>
        ///     Get calendar controls
        /// </summary>
        /// <param name="controlsList"></param>
        /// <returns></returns>
        public static string CalendarControls(List<string> controlsList)
        {
            var controlList = string.Empty;
            if (controlsList.Any())
            {
                foreach (var leftControlParameter in controlsList)
                {
                    var itm = Sitecore.Context.Database.GetItem(leftControlParameter);
                    if (itm != null && !string.IsNullOrEmpty(itm.Fields["Meta Value"].Value))
                    {
                        if (controlList.IsNullOrEmpty())
                        {
                            controlList += itm.Fields["Meta Value"].Value;
                        }
                        else
                        {
                            controlList += "," + itm.Fields["Meta Value"].Value;
                        }
                    }
                }
            }
            return controlList;
        }
    }
}