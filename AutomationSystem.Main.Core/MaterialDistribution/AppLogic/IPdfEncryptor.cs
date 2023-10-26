using System.IO;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    /// <summary>
    /// Encrypts pdf files
    /// </summary>
    public interface IPdfEncryptor
    {
        // encrypts pdf file
        byte[] Encrypt(byte[] content, string ownerPassword, string userPassword);

        // encrypts pdf file
        MemoryStream Encrypt(Stream content, string ownerPassword, string userPassword);
    }
}
