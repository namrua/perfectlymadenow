using System.Collections.Generic;
using System.IO;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic
{
    /// <summary>
    /// Processed batch files
    /// </summary>
    public interface IBatchFileDataFetcher
    {
        BatchUploadTypeEnum BatchUploadTypeId { get; }

        // gets batch file type id
        FileTypeEnum BatchFileTypeId { get; }

        // converts data from template to matrix
        List<string[]> FetchData(Stream stream);

    }
    
}
