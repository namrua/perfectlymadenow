using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Main.Web.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    [RequireHttps]
    [Authorize]
    [HandleAdminError("PreviewController")]
    public class PreviewController : Controller
    {

        // private components
        private readonly IHomeService homeService = IocProvider.Get<IHomeService>();
        private readonly IPreviewService previewService = IocProvider.Get<IPreviewService>();


        // previews style of class
        [AuthorizeAnyEntitle(Entitle.MainClasses, Entitle.MainDistanceClasses)]
        public ActionResult ClassStyle(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByClassId(id), ViewBag);
            var backUrl = Url.Action("Style", "Class", new { id });
            var model = previewService.GetPreviewStylePageModelForClass(backUrl, id);
            return View("Style", model);
        }


        // previews style of profile
        [AuthorizeEntitle(Entitle.MainProfiles)]
        public ActionResult ProfileStyle(long id)
        {
            RegistrationPageStyleHelper.SetStyle(homeService.GetRegistrationPageStyleByProfileId(id), ViewBag);
            var backUrl = Url.Action("ClassPreference", "Profile", new { id });
            var model = previewService.GetPreviewStylePageModelForProfile(backUrl, id);
            return View("Style", model);
        }

    }

}