using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;
using CorabeuControl.Components;
using CorabeuControl.Helpers;

namespace AutomationSystem.Main.Core.Gui.Helpers
{

    /// <summary>
    /// Encapsulates primitive text helpers for main gui
    /// </summary>
    public static class MainTextHelper
    {

        #region titles
      
        // gets email title
        public static string GetEmailTemplateTitle(EmailType emailType, IEnumItem language = null)
        {
            return language == null ? emailType.Description : $"{emailType.Description} ({language.Name})";
        }

        // gets full language name
        public static string GetFullLanguageName(IEnumItem language)
        {
            return language != null ? $"{language.Description} ({ language.Name})" : "";
        }

        // gets language title
        public static string GetLanguageTitle(string title, IEnumItem language)
        {
            return language != null ? $"{title} - {GetFullLanguageName(language)}" : title;
        }

        // gets enum item language title
        public static string GetEnumItemTitle(string enumType, int itemId, IEnumItem language)
        {
            return GetLanguageTitle($"{enumType} ({itemId})", language);
        }

        // gets app item title
        public static string GetAppItemTitle(string model, string label, IEnumItem language)
        {
            return GetLanguageTitle($"{model} - {label}", language);
        }

        #endregion


        #region dates & common types

        // gets date in brief format
        public static string GetBriefDate(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return null;
            var dtValue = dateTime.Value;
            var result = $"{dtValue.Month}.{dtValue.Day}.{dtValue.Year.ToString().Substring(2)}";
            return result;
        }

        // gets date in dot separated format
        public static string GetDotDate(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return null;
            var dtValue = dateTime.Value;
            var result = $"{dtValue.Month}.{dtValue.Day}.{dtValue.Year}";
            return result;
        }

        // gets full date with year
        public static string GetFullDate(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return null;
            var culture = TextHelper.DefaultCulture;
            var result = $"{dateTime.Value.ToString("M", culture)}, {dateTime.Value.Year}";
            return result;
        }

        #endregion


        #region reports

        // gets cannonical name of report - e.g. IZI CRF CLASS - ANAK 5.18.2019
        public static string GetReportFileName(string reportType, string locationCode, DateTime? classTime)
        {
            if (reportType == null)
                return null;
            var result = new StringBuilder();
            result.Append(reportType);
            if (locationCode == null && classTime == null)
                return result.ToString();
            result.Append(" -");

            if (locationCode != null)
                result.Append($" {locationCode}");
            if (classTime.HasValue)
                result.Append($" {GetDotDate(classTime)}");
            return result.ToString();
        }

        // gets cannonical name of report with extension, e.g. IZI CRF CLASS - ANAK 5.18.2019.xlsx
        public static string GetReportFileNameWithExtension(string reportType, string locationCode, DateTime? classTime, string fileExtension)
        {
            var fileName = GetReportFileName(reportType, locationCode, classTime);
            if (fileName == null || string.IsNullOrEmpty(fileExtension))
                return fileName;
            var result = fileExtension[0] == '.'
                ? fileName + fileExtension
                : $"{fileName}.{fileExtension}";
            return result;            
        }

        #endregion


        #region classes

        // get event date
        public static string GetEventDate(DateTime eventStart, DateTime eventEnd, CultureInfo culture = null)
        {
            string result;
            culture = culture ?? TextHelper.DefaultCulture;
            var dtfi = culture.DateTimeFormat;
         
            if (eventStart.Day == eventEnd.Day && eventStart.Month == eventEnd.Month && eventStart.Year == eventEnd.Year)
            {
                result = eventStart.ToString("M", culture);
            }
            else if (eventStart.Month != eventEnd.Month)
            {
                result = $"{eventStart.ToString("M", culture)} & {eventEnd.ToString("M", culture)}";
            }
            else
            {
                var monthPattern = dtfi.MonthDayPattern;
                var mIndex = monthPattern.IndexOf("M", StringComparison.Ordinal);
                var dIndex = monthPattern.IndexOf("d", StringComparison.Ordinal);
                var dPattern = monthPattern.Contains("dd") ? "D2" : "D";
                
                result = dIndex < mIndex
                    ? $"{eventStart.Day.ToString(dPattern)} & {eventEnd.ToString("M", culture)}"
                    : $"{eventStart.ToString("M", culture)} & {eventEnd.Day.ToString(dPattern)}";                
            }                       
           
            return result;
        }

        // gets event time
        public static string GetEventTime(DateTime eventStart, DateTime eventEnd, string template = "{0} to {1}", CultureInfo culture = null)
        {
            culture = culture ?? TextHelper.DefaultCulture;
            var result = string.Format(template, eventStart.ToString("t", culture), eventEnd.ToString("t", culture));           
            return result;
        }

        // gets event days
        public static string GetEventDays(DateTime eventStart, DateTime eventEnd, string template = "{0} and {1}", CultureInfo culture = null)
        {
            culture = culture ?? TextHelper.DefaultCulture;
            var result = string.Format(template, eventStart.ToString("dddd", culture), eventEnd.ToString("dddd", culture));
            return result;
        }

        // gets one line event header
        public static string GetEventOneLineHeader(DateTime eventStart, DateTime eventEnd, string location, string type, CultureInfo culture = null)
        {
            culture = culture ?? TextHelper.DefaultCulture;
            var result = $"{GetEventDate(eventStart, eventEnd, culture)}, {location}, {type}";
            return result;
        }

        // gets language info
        public static string GetEventLanguageInfo(string originLanguage, string translationLanguage, string template = "{0} with {1}")
        {            
            var result = string.IsNullOrEmpty(translationLanguage) 
                ? originLanguage : string.Format(template, originLanguage, translationLanguage);
            return result;
        }

        #endregion

        #region emails

        public static string GetNormalizedEmail(string email)
        {
            return email.ToLower().Trim();
        }

        #endregion


        #region registrations

        /// <summary>
        /// Gets registration name
        /// </summary>       
        public static string GetRegistrationName(string studentName, string registrantName)
        {
            var result = string.IsNullOrEmpty(registrantName) ? studentName : $"{studentName} ({registrantName})";
            return result;
        }

        #endregion


        #region names and addresses       

        // gets full name address part
        public static string GetFullName(string firstName, string lastName)
        {
            if (firstName == null && lastName == null)
                return null;
            if (lastName == null)
                return firstName;
            return $"{firstName} {lastName}";
        }

        public static string GetFullNameWithEmail(string firstName, string lastName, string email)
        {
            var fullName = GetFullName(firstName, lastName);
            return $"{fullName} - {email}";

        }

        // gets street address part
        public static string GetAddressStreet(string street, string street2)
        {
            if (street == null && street2 == null)
                return null;

            var isFirst = true;
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(street))
            {
                result.Append(street);
                isFirst = false;
            }
            if (!string.IsNullOrEmpty(street2))
            {
                result.Append(isFirst ? street2 : $", {street2}");             
            }

            return result.ToString();
        }

        // gets city/state address part
        public static string GetAddressCityState(string city, string state, string zipCode)
        {
            if (city == null && state == null && zipCode == null)
                return null;

            var isFirst = true;
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(city))
            {
                result.Append(city);
                isFirst = false;
            }
            if (!string.IsNullOrEmpty(state))
            {
                result.Append(isFirst ? state : $", {state}");
                isFirst = false;
            }
            if (!string.IsNullOrEmpty(zipCode))
            {
                result.Append(isFirst ? zipCode : $" {zipCode}");
            }

            return result.ToString();
        }

        // get complete address by address database model
        public static string GetCompleteAddress(Address address, string country = null)
        {
            if (address == null)
                return null;
            var street = GetAddressStreet(AddressConvertor.ToLogicString(address.Street), address.Street2);
            var cityStateZip = GetAddressCityState(AddressConvertor.ToLogicString(address.City),
                address.State, AddressConvertor.ToLogicString(address.ZipCode));

            if (street == null && cityStateZip == null)
                return null;

            var isFirst = true;
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(street))
            {
                result.Append(street);
                isFirst = false;
            }

            if (!string.IsNullOrEmpty(cityStateZip))
            {
                result.Append(isFirst ? cityStateZip : $", {cityStateZip}");
            }

            if (!string.IsNullOrEmpty(country))
            {
                result.Append(isFirst ? country : $", {country}");
            }

            return result.ToString();
        }

        #endregion


        #region language

        // gets translation language
        public static string GetTranslation(string originLanguage, string transLanguage)
        {
            var result = string.IsNullOrEmpty(transLanguage)
                ? $"{originLanguage} only"
                : $"{originLanguage} to {transLanguage}";
            return result;
        }

        #endregion


        #region ddls 

        // gets month name
        // WARNING: months indexed from 1
        public static string GetMonthName(int month, CultureInfo culture = null)
        {
            culture = culture ?? TextHelper.DefaultCulture;
            var result = culture.DateTimeFormat.GetMonthName(month);
            return result;
        }

        // gets all months
        // WARNING: months indexed from 1
        public static List<DropDownItem> GetMonthsForDdl(CultureInfo culture = null)
        {           
            culture = culture ?? TextHelper.DefaultCulture;
            var months = culture.DateTimeFormat.MonthNames;
            var result = new List<DropDownItem>();
            for (var i = 0; i < 12; i++)
                result.Add(DropDownItem.Item(i + 1, months[i]));
            return result;
        }

        // gets years
        public static List<DropDownItem> GetYearsForDdl(int from)
        {
            var result = new List<DropDownItem>();
            for (var current = DateTime.Now.Year; current >= from; current--) 
                result.Add(DropDownItem.Item(current, current.ToString()));
            return result;
        }


        #endregion


        #region entity relation

        // gets relation to another entity
        public static HtmlString GetRelation(string entityType, long? entityId)
        {
            string resultStr;
            if (entityType != null && entityId.HasValue)
                resultStr = $"{entityType} ({entityId.Value})";
            else
                resultStr = "<em>no relation</em>";
            return new HtmlString(resultStr);
        }

        #endregion


        #region codelist texts

        // gets text for ClassState
        public static string GetClassStateText(ClassState? classState)
        {
            if (!classState.HasValue)
                return null;
            switch (classState.Value)
            {
                case ClassState.New:
                    return "new";                  
                case ClassState.InRegistration:
                    return "in registration";
                case ClassState.AfterRegistration:
                    return "after registration";
                case ClassState.Canceled:
                    return "canceled";
                case ClassState.Completed:
                    return "completed";
                default:
                    throw new ArgumentOutOfRangeException(nameof(classState));
            }
        }

        // gets capital text for ClassState
        public static string GetClassStateCapitalText(ClassState? classState)
        {
            if (!classState.HasValue)
                return null;
            switch (classState.Value)
            {
                case ClassState.New:
                    return "New";
                case ClassState.InRegistration:
                    return "In registration";
                case ClassState.AfterRegistration:
                    return "After registration";
                case ClassState.Canceled:
                    return "Canceled";
                case ClassState.Completed:
                    return "Completed";
                default:
                    throw new ArgumentOutOfRangeException(nameof(classState));
            }
        }

        // gets capital text for DistanceClassTemplateState
        public static string GetDistanceClassTemplateStateCapitalText(DistanceClassTemplateState? state)
        {
            if (!state.HasValue)
            {
                return null;
            }

            switch (state.Value)
            {
                case DistanceClassTemplateState.New:
                    return "New";
                case DistanceClassTemplateState.Approved:
                    return "Approved";
                case DistanceClassTemplateState.Completed:
                    return "Completed";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }
        }

        // gets text for DistanceClassTemplateState
        public static string GetDistanceClassTemplateStateText(DistanceClassTemplateState? state)
        {
            if (!state.HasValue)
            {
                return null;
            }

            switch (state.Value)
            {
                case DistanceClassTemplateState.New:
                    return "new";
                case DistanceClassTemplateState.Approved:
                    return "approved";
                case DistanceClassTemplateState.Completed:
                    return "completed";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }
        }

        // gets text for RegistrationState
        public static string GetRegistrationStateText(RegistrationState? registrationState)
        {
            if (!registrationState.HasValue)
                return null;

            switch (registrationState.Value)
            {
                case RegistrationState.Temporary:
                    return "temporary";
                case RegistrationState.New:
                    return "new";
                case RegistrationState.Approved:
                    return "approved";
                case RegistrationState.Canceled:
                    return "canceled";                
                default:
                    throw new ArgumentOutOfRangeException(nameof(registrationState));
            }
        }

        // gets capital text for RegistrationState
        public static string GetRegistrationStateCapitalText(RegistrationState? registrationState)
        {
            if (!registrationState.HasValue)
                return null;

            switch (registrationState.Value)
            {
                case RegistrationState.Temporary:
                    return "Temporary";
                case RegistrationState.New:
                    return "New";
                case RegistrationState.Approved:
                    return "Approved";
                case RegistrationState.Canceled:
                    return "Canceled";
                default:
                    throw new ArgumentOutOfRangeException(nameof(registrationState));
            }
        }


        // gets file type text
        public static string GetFileTypeText(FileTypeEnum? fileTypeId)
        {
            if (!fileTypeId.HasValue)
                return null;
            switch (fileTypeId.Value)
            {
                case FileTypeEnum.Generic:
                    return "Unknown type";
                case FileTypeEnum.Excel:
                    return "Excel document";
                case FileTypeEnum.Word:
                    return "Word document";
                case FileTypeEnum.Csv:
                    return "CSV file";
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileTypeId));
            }
        }

        #endregion


        #region links

        // gets external link from link (adds http:// if needed)
        public static string GetExternalLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
                return null;

            var result = link.Trim();
            var toLowerLink = result.ToLower();
            if (toLowerLink.StartsWith("http://") || toLowerLink.StartsWith("https://"))
                return result;

            result = "http://" + result;
            return result;
        }

        #endregion

        #region currencies
        public static string GetCurrencyFullName(string currencyName, string currencyCode)
        {
            return $"{currencyName} ({currencyCode})";
        }
        #endregion

    }

}
