namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Determines material downloadable by public user
    /// </summary>
    public class ClassMaterialToDownload
    {

        public string Name { get; set; }
        public bool IsMaterialAvailable { get; set; }
        public long ClassMaterialFileId { get; set; }

    }
}
