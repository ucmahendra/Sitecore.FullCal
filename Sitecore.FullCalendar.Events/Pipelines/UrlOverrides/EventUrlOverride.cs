using Sitecore.Data.Items;
using Sitecore.EventCalendar.Extensions;

namespace Sitecore.EventCalendar.Pipelines.UrlOverrides
{
    public class EventUrlOverride : BaseUrlOverride
    {
        public void Process(WildcardItemUrlArgs args)
        {
            if (args.HasOverride || !args.Item.InheritsTemplate(EventConfigSettings.EventTemplateID) || Sitecore.Context.Item == null)
            {
                return;
            }
            
            var settingItem = Sitecore.Context.Database.GetItem(EventConfigSettings.EventCalendarSettingItem);
            if (settingItem != null)
            {
                if (settingItem.Fields["Fallback Events Detail Page"] != null && !settingItem.Fields["Fallback Events Detail Page"].Value.IsNullOrEmpty())
                {
                    var fieldId = settingItem.Fields["Fallback Events Detail Page"].ID;

                    if (!fieldId.IsNull)
                    {
                        Item detailItem = GetWildcardDetailPage(EventConfigSettings.EventLandingTemplateID,
                            fieldId);

                        if (detailItem != null)
                        {
                            args.Url = GetItemUrl(detailItem, args.Item, args.UrlOptions,args);
                            args.HasOverride = true;
                        }
                    }
                }
            }
        }
    }
}
