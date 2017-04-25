using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.EventCalendar.Pipelines;
using Sitecore.Links;
using Sitecore.Pipelines;

namespace Sitecore.EventCalendar.Links
{
    public class LinkProvider : Sitecore.Links.LinkProvider
    {
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            var args = new WildcardItemUrlArgs(item, options)
            {
                BaseAction = base.GetItemUrl
            };

            CorePipeline.Run(EventCalendar.Constants.Pipelines.OverrideItemUrl, args);

            if (args.HasOverride && !string.IsNullOrEmpty(args.Url))
            {
                return args.Url;
            }

            return base.GetItemUrl(item, options);
        }

        public override UrlOptions GetDefaultUrlOptions()
        {
            var urlOptions = base.GetDefaultUrlOptions();
            urlOptions.SiteResolving = Settings.Rendering.SiteResolving;
            return urlOptions;
        }
    }
}