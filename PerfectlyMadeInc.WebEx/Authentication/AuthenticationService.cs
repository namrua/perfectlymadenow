using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PerfectlyMadeInc.WebEx.Authentication
{
    public class AuthenticationService
    {
        private static readonly HttpClient client = new HttpClient();
        public const string baseUrl = "https://webexapis.com/";
        public const string AuthorizeEndPoint = "v1/authorize";
        public const string GetAccessTokenEndPoint = "v1/access_token";
        public Task<string> GetAccessToken(string clientId, string clientSecret, string redirectURI, string code)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetAuthenticationCode(string clientId, string responseType, string scope, string state, string redirectURI)
        {
            try
            {
                string result = string.Empty;
                var parameters = $"?client_id={clientId}&response_type={responseType}&redirect_uri={redirectURI}&scope={scope}&state={state}";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var fullURL = baseUrl + AuthorizeEndPoint + parameters;
                HttpResponseMessage response = await client.GetAsync(fullURL).ConfigureAwait(false);
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
