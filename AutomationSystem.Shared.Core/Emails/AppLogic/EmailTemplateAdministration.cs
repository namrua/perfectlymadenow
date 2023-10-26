using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Core.Emails.AppLogic.Convertors;
using AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.AppLogic
{
    /// <summary>
    /// Provides email administration services
    /// </summary>
    public class EmailTemplateAdministration : IEmailTemplateAdministration
    {

        // private components
        private readonly ILocalisationService localisationService;
        private readonly IEmailDatabaseLayer emailDb;
        private readonly IEmailParameterValidatorFactory paramsValidatorFactory;
        private readonly IEmailTemplateConvertor templateConvertor;
        private readonly IEmailTemplateAdministrationCommonLogic commonLogic;
        private readonly IEmailTemplateResolver resolver;
        private readonly IEmailPermissionResolver permissionResolver;
       

        // constructor
        public EmailTemplateAdministration(
            ILocalisationService localisationService,
            IEmailDatabaseLayer emailDb,
            IEmailTemplateConvertor templateConvertor,
            IEmailParameterValidatorFactory paramsValidatorFactory,
            IEmailTemplateAdministrationCommonLogic commonLogic,
            IEmailTemplateResolver resolver,
            IEmailPermissionResolver permissionResolver)
        {
            this.localisationService = localisationService;
            this.emailDb = emailDb;
            this.templateConvertor = templateConvertor;
            this.paramsValidatorFactory = paramsValidatorFactory;
            this.commonLogic = commonLogic;
            this.resolver = resolver;
            this.permissionResolver = permissionResolver;
        }


        #region lists and details

        // gets email type summary
        public EmailTypeSummary GetEmailTypeSummary(EmailTemplateEntityId emailTemplateEntityId, HashSet<EmailTypeEnum> allowedEmailTypes = null)
        {
            // loads data
            var emailTypes = emailDb.GetEmailTypes();
            if (allowedEmailTypes != null)
            {
                emailTypes = emailTypes.Where(x => allowedEmailTypes.Contains(x.EmailTypeId)).ToList();
            }

            var emailTemplates = emailDb.GetEmailTemplatesByFilter(
                new EmailTemplateFilter{ EmailTemplateEntityId = emailTemplateEntityId, IsDefault = false });
            var emailTemplateMap = emailTemplates.GroupBy(x => x.EmailTypeId).ToDictionary(x => x.Key, y => y.ToList());

            var defaultEmailTemplates = emailDb.GetEmailTemplatesByFilter(
                new EmailTemplateFilter { IsDefault = true });
            var defaultEmailTemplateMap = defaultEmailTemplates.ToDictionary(x => x.EmailTypeId);

            // creates items
            var result = new EmailTypeSummary
            {
                Items = emailTypes.Select(x => new EmailTypeSummaryItem
                {
                    EmailTypeId = x.EmailTypeId,
                    EmailType = x.Description,
                    IsAllValid = true
                }).ToList(),
                EmailTemplateEntityId = emailTemplateEntityId
            };

            // fills counts and other properties for each entity
            foreach (var item in result.Items)
            {
                if (emailTemplateMap.TryGetValue(item.EmailTypeId, out var templatesByType))
                {
                    item.EmailCount = templatesByType.Count;
                    item.ValidEmailCount = templatesByType.Count(x => x.IsValidated);
                    item.IsAllValid = item.EmailCount == item.ValidEmailCount;
                }

                if (defaultEmailTemplateMap.TryGetValue(item.EmailTypeId, out var defaultTemplatesByType))
                {
                    item.IsLocalisable = defaultTemplatesByType.IsLocalisable;
                }
            }

            return result;
        }

        public EmailTypeSummary GetSystemEmailTypeSummary()
        {
            return GetEmailTypeSummary(new EmailTemplateEntityId());
        }


        // gets list of email templates
        public EmailTemplateList GetEmailTemplateList(EmailTypeEnum emailTypeId, EmailTemplateEntityId emailTemplateEntityId)
        {
            permissionResolver.CheckEmailTemplateIsGranted(emailTemplateEntityId, emailTypeId);

            // loads email type
            var emailType = emailDb.GetEmailTypeById(emailTypeId);
            if (emailType == null)
            {
                throw new ArgumentException($"There is no Email Type with id {emailTypeId}.");
            }

            var filter = new EmailTemplateFilter
            {
                EmailTypeId = emailTypeId,
                IsDefault = false,
                EmailTemplateEntityId = emailTemplateEntityId
            };
            var templates = emailDb.GetEmailTemplatesByFilter(filter, EmailTemplateIncludes.Language);

            var defaultTemplate = emailDb.GetDefaultEmailTemplateByType(emailTypeId);
            if (defaultTemplate == null)
            {
                throw new ArgumentException($"There is no default email template for type {emailTypeId}.");
            }

            var allLanguages = localisationService.GetAllLanguages();
            var languagesWithTemplate = new HashSet<int>(templates.Select(x => (int)x.LanguageId));

            // assembles result
            var result = new EmailTemplateList
            {
                Type = emailType,
                Items = templates.Select(templateConvertor.ConvertToEmailTemplateListItem).ToList(),
                EmailTemplateEntityId = emailTemplateEntityId
            };

            // loads languages with missing templates
            if (defaultTemplate.IsLocalisable)
            {
                result.LanguagesWithoutTemplate = allLanguages.Where(x => !languagesWithTemplate.Contains(x.Id)).ToList();
            }
            else
            {
                if (!languagesWithTemplate.Contains((int) LocalisationInfo.DefaultLanguage))
                {
                    result.LanguagesWithoutTemplate.Add(allLanguages.First(x => x.Id == (int)LocalisationInfo.DefaultLanguage));
                }
            }

            return result;
        }


        // gets email template detail
        public EmailTemplateDetail GetEmailTemplateDetail(long emailTemplateId)
        {
            // gets template, default template and parameters
            var template = GetEmailTemplateById(emailTemplateId, 
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language);
            permissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(template.EntityTypeId, template.EntityId), template.EmailTypeId);
            var defaultTemplate = emailDb.GetDefaultEmailTemplateByType(template.EmailTypeId);
            var parameters = emailDb.GetEmailParametersByIds(template.EmailTemplateParameters.Select(x => x.EmailParameterId));            

            // executes validation
            var paramsValidator = paramsValidatorFactory.GetValidatorByEmailTemplate(template, parameters);
            var validationResult = commonLogic.ValidateEmailTemplate(template.Subject, template.Text, paramsValidator);
            
            // creates email template detail
            var result = templateConvertor.ConvertToEmailTemplateDetail(template, parameters);
            result.ValidationResult = validationResult;
            result.DefaultEmailTemplateId = defaultTemplate.EmailTemplateId;
            return result;
        }


        // resets email template
        public void ResetEmailTemplate(long emailTemplateId)
        {
            // gets email template and parent email template
            var template = GetEmailTemplateById(emailTemplateId, EmailTemplateIncludes.EmailTemplateParameter);
            permissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(template.EntityTypeId, template.EntityId), template.EmailTypeId);
            var parentTemplate = resolver.GetParentTemplate(template, EmailTemplateIncludes.EmailTemplateParameter);

            // updates template
            templateConvertor.ResetEmailTemplate(template, parentTemplate);

            var templateParameters = emailDb.GetEmailParametersByIds(template.EmailTemplateParameters.Select(x => x.EmailParameterId));
            var paramsValidator = paramsValidatorFactory.GetValidatorByEmailTemplate(template, templateParameters);
            var validationInfo = commonLogic.ValidateEmailTemplate(template.Subject, template.Text, paramsValidator);
            template.IsValidated = validationInfo.IsValid;

            emailDb.UpdateEmailTemplate(template);
        }

        #endregion


        #region email template metadata editing

        // get metadata for new template editation
        public EmailTemplateMetadataForEdit GetNewEmailTemplateMetadataForEdit(EmailTypeEnum emailTypeId, LanguageEnum languageId, EmailTemplateEntityId emailTemplateEntityId)
        {
            permissionResolver.CheckEmailTemplateIsGranted(emailTemplateEntityId, emailTypeId);
            var parentTemplate = resolver.GetParentTemplate(
                emailTemplateEntityId,
                languageId,
                emailTypeId,
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType);
            var result = InitializeEmailTemplateMetadataForEditNewTemplate(emailTypeId, languageId, emailTemplateEntityId, parentTemplate);                          
            result.Form = templateConvertor.ConvertToEmailTemplateMetadataForm(parentTemplate, fromParentTemplate: true);
            result.Form.LanguageId = languageId;
            result.Form.EntityId = emailTemplateEntityId.Id;
            result.Form.EntityTypeId = emailTemplateEntityId.TypeId;
            return result;
        }

        // get metadata for edittation of existing template
        public EmailTemplateMetadataForEdit GetEmailTemplateMetadataForEditById(long emailTemplateId)
        {
            // gets template and default template
            var template = GetEmailTemplateById(emailTemplateId, 
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language);
            permissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(template.EntityTypeId, template.EntityId), template.EmailTypeId);
            var parentTemplate = resolver.GetParentTemplate(template);

            // initializes assembles result
            var result = templateConvertor.InitializeEmailTemplateMetadataForEdit(template);
            result.Form = templateConvertor.ConvertToEmailTemplateMetadataForm(template);
            result.Form.ParentEmailTemplateId = parentTemplate.EmailTemplateId;                      
            return result;
        }


        // gets metadata for editation by form
        public EmailTemplateMetadataForEdit GetEmailTemplateMetadataForEditByForm(EmailTemplateMetadataForm form)
        {
            permissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(form.EntityTypeId, form.EntityId), form.EmailTypeId);
            if (form.EmailTemplateId == 0)
            {
                var parentTemplate = emailDb.GetEmailTemplateById(form.ParentEmailTemplateId);
                var result = InitializeEmailTemplateMetadataForEditNewTemplate(
                    form.EmailTypeId,
                    form.LanguageId,
                    new EmailTemplateEntityId(form.EntityTypeId, form.EntityId),
                    parentTemplate);               
                result.Form = form;
                return result;
            }
            else
            {
                // gets template
                var template = GetEmailTemplateById(form.EmailTemplateId,
                    EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language);

                // initializes email template
                var result = templateConvertor.InitializeEmailTemplateMetadataForEdit(template);
                result.Form = form;
                return result;
            }           
        }


        // updates new template
        public long SaveEmailTemplateMetadata(EmailTemplateMetadataForm form)
        {
            permissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(form.EntityTypeId, form.EntityId), form.EmailTypeId);
            // gets default template and its parameters
            var parentTemplate = emailDb.GetEmailTemplateById(form.ParentEmailTemplateId, EmailTemplateIncludes.EmailTemplateParameter);            
            var defaultParameters = emailDb.GetEmailParametersByIds(parentTemplate.EmailTemplateParameters.Select(x => x.EmailParameterId));

            // try to gets template or checks that template with type and language not exists
            EmailTemplate template = null;
            if (form.EmailTemplateId == 0)
            {
                CheckTemplateWithTypeAndLanguageNotExist(
                    form.EmailTypeId,
                    form.LanguageId,
                    parentTemplate.IsLocalisable,
                    new EmailTemplateEntityId(form.EntityTypeId, form.EntityId));
            }
            else
            {
                template = GetEmailTemplateById(form.EmailTemplateId);
            }                        
                      
            // creates dbTemplate and validates it
            var dbTemplate = templateConvertor.ConvertToEmailTemplate(form, parentTemplate, defaultParameters, template);           
            var paramsValidator = paramsValidatorFactory.GetValidatorByEmailTemplate(dbTemplate, defaultParameters);
            var validationInfo = commonLogic.ValidateEmailTemplate(dbTemplate.Subject, dbTemplate.Text, paramsValidator);
            dbTemplate.IsValidated = validationInfo.IsValid;

            // insert or update parameters
            var result = dbTemplate.EmailTemplateId;
            if (form.EmailTemplateId == 0)
            {
                result = emailDb.InsertEmailTemplate(dbTemplate);
            }
            else
            {
                emailDb.UpdateEmailTemplateMetadata(dbTemplate);
            }

            return result;
        }

        #endregion


        #region private methods - validation and condition checking

        // check wheter template with same type and language does not exist
        private void CheckTemplateWithTypeAndLanguageNotExist(EmailTypeEnum emailTypeId, LanguageEnum languageId, bool isLocalisable, EmailTemplateEntityId emailTemplateEntityId)
        {
            if (!isLocalisable && languageId != LocalisationInfo.DefaultLanguage)
            {
                throw new ArgumentException($"Language {languageId} is not supported because email template is not localisable.");
            }

            var filter = new EmailTemplateFilter
            {
                IsDefault = false,
                EmailTypeId = emailTypeId,
                LanguageId = languageId,
                EmailTemplateEntityId = emailTemplateEntityId
            };
            var templates = emailDb.GetEmailTemplatesByFilter(filter);
            if (templates.Any())
            {
                throw new InvalidOperationException(
                    $"Email template with type {emailTypeId} and language {languageId} already exists.");
            }
        }

        #endregion


        #region private methods - helpers for db loading and checking       
        
        // gets email template by id and tests it is exist
        private EmailTemplate GetEmailTemplateById(long emailTemplateId, EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            // gets default template
            var result = emailDb.GetEmailTemplateById(emailTemplateId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Email template with id {emailTemplateId}.");
            }

            return result;
        }


        // gets Language and checks that it is without email template for given type
        private IEnumItem GetLanguageWithoutEmailTemplate(EmailTypeEnum emailTypeId, LanguageEnum languageId, bool isLocalisable, EmailTemplateEntityId emailTemplateEntityId)
        {
            if (!isLocalisable && languageId != LocalisationInfo.DefaultLanguage)
            {
                throw new ArgumentException($"Language {languageId} is not supported because email template is not localisable.");
            }

            var filter = new EmailTemplateFilter
            {
                IsDefault = false,
                EmailTypeId = emailTypeId,
                EmailTemplateEntityId = emailTemplateEntityId
            };
            var languagesWithEmail = emailDb.GetLanguageIdsForEmailType(filter);

            var result = localisationService.GetAllLanguages().FirstOrDefault(x => x.Id == (int)languageId);
            if (result == null || languagesWithEmail.Contains(languageId))
            {
                throw new ArgumentException($"Language {languageId} is not supported or has already email template.");
            }

            return result;
        }       


        // initializes email template metadata for edit for nonexisting template
        private EmailTemplateMetadataForEdit InitializeEmailTemplateMetadataForEditNewTemplate(
            EmailTypeEnum emailTypeId,
            LanguageEnum languageId,
            EmailTemplateEntityId emailTemplateEntityId,
            EmailTemplate parentTemplate)
        {
            // loads language and checks there is no email template with language
            var language = GetLanguageWithoutEmailTemplate(emailTypeId, languageId, parentTemplate.IsLocalisable, emailTemplateEntityId);

            // assembles result - form model
            var result = templateConvertor.InitializeEmailTemplateMetadataForEdit(parentTemplate, fromParentTemplate: true);
            result.Language = language;           
            return result;
        }

        #endregion

    }

}
