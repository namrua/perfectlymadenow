using CorabeuControl.Helpers;
using System;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Reports.AppLogic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Web.Filters;

namespace AutomationSystem.Main.Web.Controllers
{
    [Authorize]
    [HandleAdminError("ReportController", IncidentTypeEnum.ReportError)]    
    public class ReportController : Controller
    {

        // private fields
        private readonly Lazy<string> rootPath;

        // private components        
        private readonly IGeneralReportService generalReportService = IocProvider.Get<IGeneralReportService>();

        // gets message container 
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        // constructor
        public ReportController()
        {
            rootPath = new Lazy<string>(() => Server.MapPath(ReportConstants.ReportRootPath));
        }


        // shows WWA reports tab
        [AuthorizeEntitle(Entitle.MainDistanceClasses)]
        public ActionResult Wwa()
        {
            var model = generalReportService.GetNewWwaCrfReportForEdit();
            return View(model);
        }


        // generates and downloads WWA CRF report 
        [HttpPost]
        [AuthorizeEntitle(Entitle.MainDistanceClasses)]
        public ActionResult Wwa(WwaCrfReportForm form)
        {
            // validates form
            if (!ModelState.IsValid || form.FromDate > form.ToDate)
            {
                ViewBag.TriggerValidation = true;
                var model = generalReportService.GetWwaCrfReportForEditFromForm(form);
                return View(model);
            }

            var fileToDownload = generalReportService.GenerateWwaCrfReport(rootPath.Value, form);
            return fileToDownload.GetFileActionResult();
        }


        // downloads crf report
        [AuthorizeEntitle(Entitle.MainClasses)]
        public ActionResult Download(long id, ClassReportType reportType)
        {
            try
            {
                var fileForDownload = generalReportService.GetClassReportByType(reportType, rootPath.Value, id);
                return fileForDownload.GetFileActionResult();
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToAction("Index", "Class");
            }
        }      

    }

}