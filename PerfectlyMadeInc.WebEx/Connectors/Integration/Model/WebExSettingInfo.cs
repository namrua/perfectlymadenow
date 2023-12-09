
namespace PerfectlyMadeInc.WebEx.Connectors.Integration.Model
{
    /// <summary>
    /// Webex types class
    /// </summary>
    public class WebExSettingInfo
    {
        // public properties
        public string SiteName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }
        public bool Enabled { get; set; }
        public string AccessToken { get; set; }

        // constructor
        public WebExSettingInfo() { }
        public WebExSettingInfo(string siteName, string login, string password, string serviceUrl, bool enabled)
        {
            SiteName = siteName;
            Login = login;
            Password = password;
            ServiceUrl = serviceUrl;
            Enabled = enabled;
        }
    }  
    
}
