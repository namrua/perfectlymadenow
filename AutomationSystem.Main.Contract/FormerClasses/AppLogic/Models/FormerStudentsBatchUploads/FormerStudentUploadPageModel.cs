using System.Collections.Generic;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads
{
    /// <summary>
    /// Former student upload page model
    /// </summary>
    public class FormerStudentUploadPageModel
    {

        public long FormerClassId { get; set; }
        public List<BatchUploadListItem> Items { get; set; }

        // constructor
        public FormerStudentUploadPageModel()
        {
            Items = new List<BatchUploadListItem>();
        }

    }

}