using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{

    /// <summary>
    /// Converts class style objects
    /// </summary>
    public class ClassStyleConvertor : IClassStyleConvertor
    {
        private readonly Lazy<Dictionary<RegistrationColorSchemeEnum, RegistrationColorScheme>> colorSchemeMap;


        // constructor
        public ClassStyleConvertor(IClassDatabaseLayer classDb)
        {
            colorSchemeMap = new Lazy<Dictionary<RegistrationColorSchemeEnum, RegistrationColorScheme>>(
                () => classDb.GetRegistrationColorSchemes().ToDictionary(x => x.RegistrationColorSchemeId));
        }


        // creates class business entity by class preference
        public ClassStyle CreateClassStyleByClassPreference(ClassPreference classPreference, bool showClassBehaviorSettings)
        {
            var result = new ClassStyle
            {
                HomepageUrl = classPreference.HomepageUrl,
                HeaderPictureId = classPreference.HeaderPictureId,
                RegistrationColorSchemeId = classPreference.RegistrationColorSchemeId,
                SendCertificatesByEmail = classPreference.SendCertificatesByEmail
            };

            // SendCertificatesByEmail when class behavior is not shown
            if (!showClassBehaviorSettings)
            {
                result.SendCertificatesByEmail = true;
            }
            return result;
        }


        // initialize ClassStyleForEdit
        public ClassStyleForEdit InitializeClassStyleForEdit(bool showClassBehaviorSettings)
        {
            var result = new ClassStyleForEdit();
            result.ColorSchemes = colorSchemeMap.Value.Values.ToList();
            result.ShowClassBehaviorSettings = showClassBehaviorSettings;
            return result;
        }

        // converts ClassStyle to ClassStyleForm
        public ClassStyleForm ConvertToClassStyleForm(ClassStyle classStyle, long classId)
        {
            var result = new ClassStyleForm
            {
                ClassId = classId,
                HomepageUrl = classStyle.HomepageUrl,
                OriginHeaderPictureId = classStyle.HeaderPictureId,
                RegistrationColorSchemeId = classStyle.RegistrationColorSchemeId,
                SendCertificatesByEmail = classStyle.SendCertificatesByEmail
            };
            return result;
        }

        // converts ClassStyle to ClassStyleDetail
        public ClassStyleDetail ConvertToClassStyleDetail(ClassStyle classStyle, bool showClassBehaviorSettings)
        {
            var result = new ClassStyleDetail
            {
                HomepageUrl = classStyle.HomepageUrl,
                HeaderPictureId = classStyle.HeaderPictureId,
                RegistrationColorSchemeId = classStyle.RegistrationColorSchemeId,
                RegistrationColorScheme = colorSchemeMap.Value[classStyle.RegistrationColorSchemeId].Description,
                SendCertificatesByEmail = classStyle.SendCertificatesByEmail,
                ShowClassBehaviorSettings = showClassBehaviorSettings
            };
            return result;
        }
        

        // converts ClassStyleForm to ClassStyle
        public ClassStyle ConverToClassStyle(ClassStyleForm form, long? headerPictureId)
        {
            var result = new ClassStyle
            {
                HomepageUrl = MainTextHelper.GetExternalLink(form.HomepageUrl),
                HeaderPictureId = headerPictureId,
                RegistrationColorSchemeId = form.RegistrationColorSchemeId ?? 0,
                SendCertificatesByEmail = form.SendCertificatesByEmail,
            };
            return result;
        }

    }

}
