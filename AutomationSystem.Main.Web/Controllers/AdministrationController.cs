using System.Web.Mvc;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    // warning: dont use HandleAdminErrorAttribute here - Error action should be unmarked to propagate error to Global.asax
    public class AdministrationController : Controller
    {

        // index
        public ActionResult Index()
        {
            return View();
        }


        // access denied page
        public ActionResult AccessDenied()
        {
            return View();
        }


        // Shows error with link to incident
        // warning: dont use HandleAdminErrorAttribute here - error action should be unmarked to propagate error to Global.asax
        public ActionResult Error(long? id)
        {
            ViewBag.IncidentId = id;
            return View();
        }
       
    }

}