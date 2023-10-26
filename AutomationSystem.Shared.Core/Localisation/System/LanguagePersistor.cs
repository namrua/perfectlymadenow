using System;
using System.Globalization;
using System.Threading;
using System.Web;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Localisation.System
{
    /// <summary>
    /// Provides persistence of language
    /// </summary>
    public class LanguagePersistor : ILanguagePersistor
    {

        // constants
        private const string defaultLanguageCode = LocalisationInfo.DefaultLanguageCode;
        private const string cookieKey = "CurrentLanguage";
        private const string sessionKey = "Localisation.CurrentLanguage";
        
        // private components
        private readonly ITracer tracer;


        // gets current language code
        public string CurrentLanguageCode => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        // get currrent culture info
        public CultureInfo CurrentCultureInfo => Thread.CurrentThread.CurrentUICulture;

        public LanguagePersistor(ITracerFactory tracerFactory)
        {
            tracer = tracerFactory.CreateTracer<LanguagePersistor>();
        }

        // initializes language code
        public void Initialize()
        {
            var currentCultureInfo = GetCultureInfo();
            Thread.CurrentThread.CurrentUICulture = currentCultureInfo;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentCultureInfo.Name);
        }


        // changes current language
        public void ChangeLanguage(string languageCode, bool saveCookies = true)
        {
            try
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    SetCodeToSession(context, languageCode);
                    if (saveCookies)
                        SetCodeToCookie(context, languageCode);
                }

                Initialize();
            }
            catch (Exception e)
            {
                tracer.Error(e);
            }
        }


        #region private methods

        // loads current culture info from session
        private CultureInfo GetCultureInfo()
        {
            string currentLanguage = null;
            try
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    currentLanguage = GetCodeFromSession(context);
                    if (currentLanguage == null)
                    {
                        currentLanguage = GetCodeFromCookie(context);
                        if (currentLanguage != null)
                            SetCodeToSession(context, currentLanguage);
                    }
                }
            }
            catch (Exception e)
            {
                tracer.Error(e);
            }
            return new CultureInfo(currentLanguage ?? defaultLanguageCode);
        }
     

        // gets current language code from session
        private string GetCodeFromSession(HttpContext context)
        {            
            return (context.Session != null) ? HttpContext.Current.Session[sessionKey] as string : null;
        }

        // sets current language to session
        private void SetCodeToSession(HttpContext context, string value)
        {            
            if (context.Session != null)
                context.Session[sessionKey] = value;
        }


        // gets current language code from cookie
        private string GetCodeFromCookie(HttpContext context)
        {        
            var cookie = context.Request.Cookies[cookieKey];
            return cookie?.Value;

        }

        // sets current language to cookie
        private static void SetCodeToCookie(HttpContext context, string value)
        {         
            var cookie = new HttpCookie(cookieKey);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddMonths(6);
            context.Response.Cookies.Add(cookie);         
        }

        #endregion

    }
  
}
