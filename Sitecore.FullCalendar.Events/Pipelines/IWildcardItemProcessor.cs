namespace Sitecore.EventCalendar.Pipelines
{
    public interface IWildcardItemProcessor
    {
        void Process(WildcardItemUrlArgs args);
    }
}