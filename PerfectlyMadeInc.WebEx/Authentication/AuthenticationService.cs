using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static PerfectlyMadeInc.WebEx.Helper.Constants;

namespace PerfectlyMadeInc.WebEx.Authentication
{
    public class AuthenticationService : IAuthentication
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ITracerFactory _tracerFactory;
        private ITracer tracer;

        public AuthenticationService(ITracerFactory tracerFactory)
        {
            _tracerFactory = tracerFactory;
        }
        public Task<string> GetAccessToken(string clientId, string clientSecret, string redirectURI, string code)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetAuthenticationCode(string clientId, string responseType, string scope, string state, string redirectURI)
        {
            try
            {
                tracer = _tracerFactory.CreateTracer<AuthenticationService>();
                var parameters = $"?client_id={clientId}&response_type={responseType}&redirect_uri={redirectURI}&scope={scope}&state={state}";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var fullURL = WebexUrls.BaseUrl + WebexUrls.Authorize + parameters;
                HttpResponseMessage response = await client.GetAsync(fullURL).ConfigureAwait(false);
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseBody;
            }
            catch (Exception ex)
            {
                tracer.Error(ex, "Authentication failure");
                throw;
            }
        }
    }
}
