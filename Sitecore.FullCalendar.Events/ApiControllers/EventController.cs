using System;
using System.Web.Mvc;
using Sitecore.EventCalendar.EventModels;
using Sitecore.EventCalendar.Extensions;
using Sitecore.EventCalendar.Managers;

namespace Sitecore.EventCalendar.ApiControllers
{
    public class EventController : Controller
    {
        [HttpPost]
        public ActionResult GetEvents(EventParam param)
        {
            try
            {
                if (param != null && !param.ItemId.IsNullOrEmpty())
                {
                    Sitecore.Context.Item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(param.ItemId));
                }
                var EventResult = EventSearch.GetEvents(param);
                var s = EventResult.ToJson();
                return Json(s, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception e)
            {
                return Json(new {success = false, ex = e.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}