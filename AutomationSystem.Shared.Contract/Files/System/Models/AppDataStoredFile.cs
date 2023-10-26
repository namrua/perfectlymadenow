namespace AutomationSystem.Shared.Contract.Files.System.Models
{
    /// <summary>
    /// Encapsulates informations about AppData store file
    /// </summary>
    public class AppDataStoredFile
    {

        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileMimeType { get; set; }        

        // constructor
        public AppDataStoredFile() { }
        public AppDataStoredFile(string path, string fileName, string fileMimeType)
        {
            Path = path;
            FileName = fileName;
            FileMimeType = fileMimeType;
        }

    }

}
