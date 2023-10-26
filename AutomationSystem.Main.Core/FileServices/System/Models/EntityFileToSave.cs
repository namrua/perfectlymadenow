using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.FileServices.System.Models
{
    public class EntityFileToSave
    {

        public long EntityId { get; set; }
        public EntityTypeEnum EntityTypeId { get; set; }                // serves for slow replacement of file entities to one core entity
        public string Code { get; set; }
        public string DisplayedName { get; set; }
        public string FileName { get; set; }
        public FileTypeEnum FileTypeId { get; set; }
        public byte[] Content { get; set; }        

    }

}
