using Sitecore.Configuration;

namespace Sitecore.EventCalendar
{
   public class EventConfigSettings
    {
        public static string EventTemplateID = Settings.GetSetting("EventTemplateID");
        public static string EventCalendarRootID = Settings.GetSetting("EventCalendarRootID");
        public static string EventCategoryFolderID = Settings.GetSetting("EventCategoryFolderID");
        public static string EventCategoryItemTemplateID = Settings.GetSetting("EventCategoryItemTemplateID");      
        public static string EventLandingTemplateID = Settings.GetSetting("EventLandingTemplateID");
        public static string EventCalendarSettingItem = Settings.GetSetting("EventCalendarSettingItem");
    }
}
