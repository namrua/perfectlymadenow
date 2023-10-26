using System;
using System.Web.Mvc;
using System.Web.UI;
using AutomationSystem.Main.Contract.Files.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Web.Filters;
using AutomationSystem.Shared.Contract.Files.System;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Web.Controllers
{
    // provides fields form system
    [Authorize]
    [HandleAdminError("FileController")]
    public class FileController : Controller
    {

        // private components              
        private readonly ICoreFileService coreFileService = IocProvider.Get<ICoreFileService>();
        private readonly IMainFileService mainFileService = IocProvider.Get<IMainFileService>();

        // private fields
        private readonly Lazy<string> rootPath;

        // gets message container
        public IMessageContainer MessageContainer => new MessageContainer(Session);

        public FileController()
        {
            rootPath = new Lazy<string>(() => Server.MapPath("~/App_Data"));
        }


        // download public file
        [AllowAnonymous]
        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Client)]
        public ActionResult Image(long id)
        {
            // WARNING: be very careful here not to open some security issue !!!
            var file = coreFileService.GetFileForPublicDownloadById(id);
            return file.GetFileActionResult();
        }


        // download file
        public ActionResult Download(long id)
        {       
            // todo: #SECURITY - access privileges checking
            var file = coreFileService.GetFileForDownloadById(id);
            return file.GetFileActionResult();          
        }  
        

        // download stored file
        public ActionResult DownloadMainStored(MainStoredFile id)
        {
            var file = mainFileService.GetStoredFile(rootPath.Value, id);
            return file.GetFileActionResult();
        }

    }

}