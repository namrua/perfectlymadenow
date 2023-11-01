using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationSystem.Main.Web.Helpers
{
    public static class Constants
    {
        public static class WebexUrls
        {
            /// <summary>
            /// get authorize from webex with clienId, clientSecret, and Scope as known as Roles
            /// </summary>
            public const string Authorize = "v1/authorize";
            public const string GetAccessToken = "v1/access_token";
        }
    }
}