using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.EventCalendar.Extensions;
using Sitecore.Links;

namespace Sitecore.EventCalendar.Pipelines.UrlOverrides
{
    public class BaseUrlOverride
    {
        private string GetFriendlyUrl(Item item)
        {
            return item.Name.ToLower().Replace(" ", "-");
        }

        protected string GetItemUrl(Item detailItem, Item targetItem, UrlOptions urlOptions, WildcardItemUrlArgs args)
        {
            var friendlyUrl = GetFriendlyUrl(targetItem);
            return args.BaseAction(detailItem, urlOptions)
                .Replace(Constants.Wildcard.UrlToken, friendlyUrl)
                .Replace(Constants.Wildcard.Node, friendlyUrl);
        }

        protected Item GetWildcardDetailPage(string landingPageTemplateId, ID fieldId, Item currentItem = null)
        {
            Item detailItem = null;

            var context = Context.Item;

            // Handle case where context page is either the wildcard node or landing page
            if (context.Name.StartsWith(Constants.Wildcard.Node) &&
                context.Parent.InheritsTemplate(landingPageTemplateId))
            {
                detailItem = context;
            }
            else if (context.InheritsTemplate(landingPageTemplateId))
            {
                detailItem =
                    context.Children
                        .FirstOrDefault(i => i.Name.StartsWith(Constants.Wildcard.Node));
            }

            if (detailItem == null)
            {
                var settingItem = Sitecore.Context.Database.GetItem(EventConfigSettings.EventCalendarSettingItem);
                if (settingItem != null)
                {
                    var siteLookupField = new LookupField(settingItem.Fields[fieldId]);
                    var selectedPage = siteLookupField.TargetItem;
                    if (selectedPage != null && selectedPage.Name.StartsWith(EventCalendar.Constants.Wildcard.Node))
                    {
                        detailItem = selectedPage;
                    }
                }
            }

            return detailItem;
        }
    }
}