namespace AutomationSystem.Shared.Contract.BatchUploads.System
{
    public interface IBatchUploadValueResolver
    {
        string[] GetValues(string[] origValues);
    }
}
