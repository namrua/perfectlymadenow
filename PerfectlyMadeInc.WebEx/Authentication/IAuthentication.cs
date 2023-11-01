using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfectlyMadeInc.WebEx.Authentication
{
    public interface IAuthentication
    {
        Task<string> GetAuthenticationCode(string clientId, string responseType, string scope, string state, string redirectURI);
        Task<string> GetAccessToken(string clientId, string clientSecret, string redirectURI, string code);
    }
}
