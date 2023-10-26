using System.Collections.Generic;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models
{
    /// <summary>
    ///  Batch upload for edit
    /// </summary>
    public class BatchUploadForEdit<TForm> where TForm : new()
    {

        // properties
        public TForm Form { get; set; } = new TForm();
        public List<BatchUploadType> FileTypes { get; set; }

        // constructor
        public BatchUploadForEdit()
        {
            FileTypes = new List<BatchUploadType>();
        }

    }
}
