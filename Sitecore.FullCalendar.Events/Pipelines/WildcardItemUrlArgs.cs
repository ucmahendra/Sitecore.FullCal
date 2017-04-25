using System;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Pipelines;

namespace Sitecore.EventCalendar.Pipelines
{
    public class WildcardItemUrlArgs : PipelineArgs
    {
        public WildcardItemUrlArgs(Item item, UrlOptions urlOptions)
        {
            Item = item;
            UrlOptions = urlOptions;
        }

        public Item Item { get; private set; }
        public UrlOptions UrlOptions { get; private set; }
        public string Url { get; set; }
        public bool HasOverride { get; set; }
        public Func<Item, UrlOptions, string> BaseAction { get; set; }
    }
}