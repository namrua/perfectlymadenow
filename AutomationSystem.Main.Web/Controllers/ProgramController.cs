using CorabeuControl.Helpers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Integration.AppLogic;
using AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs;
using AutomationSystem.Main.Web.Filters;

namespace AutomationSystem.Main.Web.Controllers
{
    /// <summary>
    /// Controller for program administration
    /// </summary>
    [RequireHttps]
    [Authorize]
    [HandleAdminError("ProgramController")]
    [AuthorizeEntitle(Entitle.WebExPrograms)]
    public class ProgramController : Controller
    {

        // private components 
        private readonly IMainProgramAdministration programAdministration = IocProvider.Get<IMainProgramAdministration>();

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);


        // show list of programs
        public ActionResult Index(MainProgramFilter filter, bool search = false)
        {
            var model = programAdministration.GetMainProgramListPageModel(filter, search);
            return View(model);
        }

        // program detail 
        public ActionResult Detail(long id)
        {
            try
            {
                var model = programAdministration.GetProgramById(id);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

        // page for loading of new program
        public ActionResult New(long profileId)
        {
            try
            {
                var model = programAdministration.GetNewProgramModel(profileId);
                return View(model);
            }
            catch (ArgumentException e)
            {
                MessageContainer.PushErrorMessage(e.Message);
                return RedirectToHome();
            }
        }

       
        // loads list of WebEx programs asynchronously
        [HttpPost]
        public async Task<ActionResult> LoadList(long accountId)
        {
            try
            {
                //var model = await programAdministration.GetNewProgramsForList(accountId);
                //return PartialView("_TablePartial", model);
                var model = await programAdministration.GetNewWebinarsForList(accountId);
                return PartialView("_TablePartialWebinar", model);
            }
            catch (Exception e)
            {                
                return new HttpStatusCodeResult(500, e.Message);
            }
        }


        // adss program
        [HttpPost]
        public async Task<ActionResult> Add(long programOuterId, long accountId)
        {
            try
            {
                var id = await programAdministration.SaveProgramFromWebEx(programOuterId, accountId);
                return Json(id);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
        }


        // deletes program
        [HttpPost]
        public ActionResult Delete(int id)
        {
            programAdministration.DeleteProgram(id);
            MessageContainer.PushMessage("Program was deleted.");
            return RedirectToHome();
        }


        #region

        // redirects to controller's homepage
        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Index");
        }

        #endregion

    }

}