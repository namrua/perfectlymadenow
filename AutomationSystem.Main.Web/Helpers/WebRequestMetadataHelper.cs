using System.Web;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;

namespace AutomationSystem.Main.Web.Helpers
{
    /// <summary>
    /// Provides WebRequest metdatada
    /// </summary>
    public static class WebRequestMetadataHelper
    {

        // gets WebRequest info
        public static WebRequestInfo GetWebRequestInfo(HttpRequestBase request)
        {
            var result = new WebRequestInfo();
            if (request == null)
                return result;

            // assembles result
            result.IpAddress = GetIpAddress(request);
            return result;
        }


        // gets IP address of request
        public static string GetIpAddress(HttpRequestBase request)
        {
            var ipAddresses = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddresses))
            {
                string[] addresses = ipAddresses.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return request.ServerVariables["REMOTE_ADDR"];
        }

    }

}