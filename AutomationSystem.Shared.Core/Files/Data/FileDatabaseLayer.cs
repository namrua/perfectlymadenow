using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Shared.Core.Files.Data.Extensions;
using AutomationSystem.Shared.Core.Files.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Files.Data
{
    /// <summary>
    /// File database layer
    /// </summary>
    public class FileDatabaseLayer : IFileDatabaseLayer
    {

        private const int fileContentManipulationTimeout = 600;             // determines timeout for content manipulation in seconds


        // gets files by list of ids
        public List<File> GetFilesById(List<long> ids, FileIncludes includes = FileIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Files.AddIncludes(includes).Active().Where(x => ids.Contains(x.FileId)).ToList();
                return result;
            }
        }

        // gets file by file id
        public File GetFileById(long fileId, FileIncludes includes = FileIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Files.AddIncludes(includes).Active().FirstOrDefault(x => x.FileId == fileId);
                return result;
            }

        }

        // get files by ids
        public List<File> GetFilesByIds(IEnumerable<long> fileIds, FileIncludes includes = FileIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Files.AddIncludes(includes).Active().Where(x => fileIds.Contains(x.FileId)).ToList();
                return result;
            }
        }

        // insert file
        public long InsertFile(File file)
        {
            using (var context = new CoreEntities())
            {
                context.Database.CommandTimeout = fileContentManipulationTimeout;

                context.Files.Add(file);
                context.SaveChanges();
                return file.FileId;
            }

        }

        // updates file content
        public void UpdateFileContent(long fileId, byte[] content, string name = null, string fileName = null)
        {
            using (var context = new CoreEntities())
            {
                context.Database.CommandTimeout = fileContentManipulationTimeout;

                var toUpdate = context.Files.Active().FirstOrDefault(x => x.FileId == fileId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no File with id {fileId}.");

                toUpdate.Name = name ?? toUpdate.Name;
                toUpdate.FileName = fileName ?? toUpdate.FileName;
                toUpdate.Data = content;
               
                context.SaveChanges();                
            }
        }


        // clone existing file 
        public long CloneFile(long fileId, string name)
        {
            using (var context = new CoreEntities())
            {
                context.Database.CommandTimeout = fileContentManipulationTimeout;

                // gets object with AsNoTracking() 
                var toClone = context.Files.AsNoTracking().Active().FirstOrDefault(x => x.FileId == fileId);
                if (toClone == null)
                    throw new ArgumentException($"There is no File with id {fileId}.");

                // resets/presets parameters
                toClone.FileId = 0;
                toClone.Name = name;

                // incerts new object
                context.Files.Add(toClone);
                context.SaveChanges();

                return toClone.FileId;
            }
        }


        // delete file
        public void DeleteFile(long fileId)
        {
            using (var context = new CoreEntities())
            {
                var toDelete = context.Files.Active().FirstOrDefault(x => x.FileId == fileId);
                if (toDelete == null) return;

                context.Files.Remove(toDelete);
                context.SaveChanges();
            }
        }

    }

}
