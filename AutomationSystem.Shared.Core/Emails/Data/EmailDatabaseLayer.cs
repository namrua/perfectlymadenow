using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Core.Emails.Data.Extensions;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.Emails.Data
{
    /// <summary>
    /// Database layer for email administration
    /// </summary>
    public class EmailDatabaseLayer : IEmailDatabaseLayer
    {
        private const int EmailSelectTimeout = 600;             // determines timeout in seconds

        #region email administration

        // gets email types
        public List<EmailType> GetEmailTypes()
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailTypes.ToList();
                return result;
            }
        }

        // gets email type by id
        public EmailType GetEmailTypeById(EmailTypeEnum emailTypeId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailTypes.FirstOrDefault(x => x.EmailTypeId == emailTypeId);
                return result;
            }
        }

        // get language ids for email type
        public HashSet<LanguageEnum> GetLanguageIdsForEmailType(EmailTemplateFilter filter)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailTemplates.Active().Filter(filter).Select(x => x.LanguageId).ToList();
                return new HashSet<LanguageEnum>(result);
            }
        }



        // gets email templates by filter
        public List<EmailTemplate> GetEmailTemplatesByFilter(EmailTemplateFilter filter = null,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailTemplates.AddIncludes(includes).Filter(filter).ToList();
                foreach (var entity in result)
                    EmailRemoveInactive.RemoveInactiveForEmailTemplate(entity, includes);
                return result;
            }
        }

        // get default email for type
        public EmailTemplate GetDefaultEmailTemplateByType(EmailTypeEnum type,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailTemplates.AddIncludes(includes).Active()
                    .FirstOrDefault(x => x.EmailTypeId == type && x.IsDefault);
                EmailRemoveInactive.RemoveInactiveForEmailTemplate(result, includes);
                return result;
            }
        }

        // get email template by id
        public EmailTemplate GetEmailTemplateById(long id, EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailTemplates.AddIncludes(includes).Active()
                    .FirstOrDefault(x => x.EmailTemplateId == id);
                EmailRemoveInactive.RemoveInactiveForEmailTemplate(result, includes);
                return result;
            }
        }

        public EmailTemplate GetEmailTemplateByEmailTemplateEntityId(EmailTemplateEntityId emailTemplateEntityId, EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailTemplates.AddIncludes(includes).Active()
                    .FirstOrDefault(x => x.EntityId == emailTemplateEntityId.Id && x.EntityTypeId == emailTemplateEntityId.TypeId);
                return result;
            }
        }

        // gets template parameters with email parameters by email template id
        public List<EmailTemplateParameter> GetEmailTemplateParametersByEmailTemplateId(long emailTemplateId,
            bool includeEmailParameters = true)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<EmailTemplateParameter> query = context.EmailTemplateParameters;
                if (includeEmailParameters)
                    query = query.Include("EmailParameter");
                var result = query.Active().Where(x => x.EmailTemplateId == emailTemplateId).ToList();
                return result;
            }
        }

        // get email parameters
        public List<EmailParameter> GetEmailParametersByIds(IEnumerable<long> ids)
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailParameters.Active().Where(x => ids.Contains(x.EmailParameterId)).ToList();
                return result;
            }
        }


        // insert email template
        public long InsertEmailTemplate(EmailTemplate emailTemplate)
        {
            using (var context = new CoreEntities())
            {
                context.EmailTemplates.Add(emailTemplate);
                context.SaveChanges();
                return emailTemplate.EmailTemplateId;
            }
        }
        

        // update email template metadata
        public void UpdateEmailTemplateMetadata(EmailTemplate emailTemplate, EmailOperationOption options = EmailOperationOption.None)
        {
            using (var context = new CoreEntities())
            {
                // updates emailTemplate
                var toUpdate = context.EmailTemplates.Active().FirstOrDefault(x => x.EmailTemplateId == emailTemplate.EmailTemplateId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Email template with id {emailTemplate.EmailTemplateId}.");

                if (!options.HasFlag(EmailOperationOption.OmitSealedCondition) && toUpdate.IsSealed)
                    throw new InvalidOperationException($"Email template with id {emailTemplate.EmailTemplateId} is sealed and cannot be updated.");
                toUpdate.FillingNote = emailTemplate.FillingNote;
                toUpdate.IsValidated = emailTemplate.IsValidated;

                // insert or updates emailTemplateParameters
                var originParamsMap = context.EmailTemplateParameters.Active()
                    .Where(x => x.EmailTemplateId == emailTemplate.EmailTemplateId);
                var setUpdateResolver = new SetUpdateResolver<EmailTemplateParameter, long>(
                    x => x.EmailParameterId, (origItem, newItem) => { origItem.IsRequired = newItem.IsRequired; });
                var strategy = setUpdateResolver.ResolveStrategy(originParamsMap, emailTemplate.EmailTemplateParameters);
                context.EmailTemplateParameters.AddRange(strategy.ToAdd);
                context.EmailTemplateParameters.RemoveRange(strategy.ToDelete);

                // saves changes
                context.SaveChanges();
            }
        }


        // update email template text
        public void UpdateEmailTemplateText(EmailTemplate emailTemplate, EmailOperationOption options = EmailOperationOption.None)
        {
            using (var context = new CoreEntities())
            {
                var toUpdate = context.EmailTemplates.Active().FirstOrDefault(x => x.EmailTemplateId == emailTemplate.EmailTemplateId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Email template with id {emailTemplate.EmailTemplateId}.");

                if (!options.HasFlag(EmailOperationOption.OmitSealedCondition) && toUpdate.IsSealed)
                    throw new InvalidOperationException($"Email template with id {emailTemplate.EmailTemplateId} is sealed and cannot be updated.");
                toUpdate.Subject = emailTemplate.Subject;
                toUpdate.Text = emailTemplate.Text;
                toUpdate.IsValidated = emailTemplate.IsValidated;
                context.SaveChanges();
            }
        }


        // updates email template
        public void UpdateEmailTemplate(EmailTemplate emailTemplate, EmailOperationOption options = EmailOperationOption.None)
        {
            using (var context = new CoreEntities())
            {
                // updates emailTemplate
                var toUpdate = context.EmailTemplates.Active().FirstOrDefault(x => x.EmailTemplateId == emailTemplate.EmailTemplateId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Email template with id {emailTemplate.EmailTemplateId}.");

                if (!options.HasFlag(EmailOperationOption.OmitSealedCondition) && toUpdate.IsSealed)
                    throw new InvalidOperationException($"Email template with id {emailTemplate.EmailTemplateId} is sealed and cannot be updated");
                toUpdate.FillingNote = emailTemplate.FillingNote;
                toUpdate.IsValidated = emailTemplate.IsValidated;
                toUpdate.Subject = emailTemplate.Subject;
                toUpdate.Text = emailTemplate.Text;

                // insert or updates emailTemplateParameters
                var originParamsMap = context.EmailTemplateParameters.Active()
                    .Where(x => x.EmailTemplateId == emailTemplate.EmailTemplateId);
                var setUpdateResolver = new SetUpdateResolver<EmailTemplateParameter, long>(
                    x => x.EmailParameterId, (origItem, newItem) => { origItem.IsRequired = newItem.IsRequired; });
                var strategy = setUpdateResolver.ResolveStrategy(originParamsMap, emailTemplate.EmailTemplateParameters);
                context.EmailTemplateParameters.AddRange(strategy.ToAdd);
                context.EmailTemplateParameters.RemoveRange(strategy.ToDelete);

                // saves changes
                context.SaveChanges();
            }
        }

        #endregion


        #region email

        // gets email for detail
        public Email GetEmailForDetailById(long emailId, EmailIncludes includes = EmailIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Emails.AddIncludes(includes).Active().FirstOrDefault(x => x.EmailId == emailId);
                return result;
            }
        }

        #endregion


        #region email administration integration

        // insert email templates
        public void InsertEmailTemplates(IEnumerable<EmailTemplate> emailTemplates)
        {
            using (var context = new CoreEntities())
            {
                context.EmailTemplates.AddRange(emailTemplates);
                context.SaveChanges();
            }
        }

        // seals email templates
        public void SetEmailTemplatesToSealed(IEnumerable<long> emailTemplateIds, EmailOperationOption options = EmailOperationOption.None)
        {
            using (var context = new CoreEntities())
            {
                var toSeal = context.EmailTemplates.Active().Where(x => emailTemplateIds.Contains(x.EmailTemplateId)).ToList();
                foreach (var template in toSeal)
                    template.IsSealed = true;
                context.SaveChanges();
            }
        }


        // delete email template
        public void DeleteEmailTemplate(long emailTemplateId, EmailOperationOption options = EmailOperationOption.None)
        {
            using (var context = new CoreEntities())
            {
                var toDelete = context.EmailTemplates.Include("Emails").Include("EmailTemplateParameters").Active().FirstOrDefault(x => x.EmailTemplateId == emailTemplateId);
                if (toDelete == null) return;
                toDelete.Emails = toDelete.Emails.AsQueryable().Active().ToList();

                // check conditions
                if (toDelete.IsDefault)
                    throw new InvalidOperationException($"Emailt template with id {emailTemplateId} is default and cannot be deleted.");
                if (options.HasFlag(EmailOperationOption.CheckNoEmails) && toDelete.Emails.Count(x => !x.IsTestEmail) > 0)
                    throw new InvalidOperationException($"Email template with id {emailTemplateId} contains non-test emails and cannot be deleted.");

                // deletes email template
                context.EmailTemplateParameters.RemoveRange(toDelete.EmailTemplateParameters);
                context.Emails.RemoveRange(toDelete.Emails);
                context.EmailTemplates.Remove(toDelete);
                context.SaveChanges();
            }
        }


        // delete email templates by entity id
        public void DeleteEmailTemplatesByEntity(EntityTypeEnum entityTypeId, long entityId, EmailOperationOption options = EmailOperationOption.None)
        {
            using (var context = new CoreEntities())
            {
                var toDeleteList = context.EmailTemplates.Include("Emails").Include("EmailTemplateParameters").Active()
                    .Where(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId).ToList();

                // deletes all email templates
                foreach (var toDelete in toDeleteList)
                {
                    toDelete.Emails = toDelete.Emails.AsQueryable().Active().ToList();

                    // check conditions
                    if (toDelete.IsDefault)
                        throw new InvalidOperationException($"Emailt template with id {toDelete.EmailTemplateId} is default and cannot be deleted.");
                    if (options.HasFlag(EmailOperationOption.CheckNoEmails) && toDelete.Emails.Count(x => !x.IsTestEmail) > 0)
                        throw new InvalidOperationException($"Email template with id {toDelete.EmailTemplateId} contains non-test emails and cannot be deleted.");

                    // deletes email template
                    context.EmailTemplateParameters.RemoveRange(toDelete.EmailTemplateParameters);
                    context.Emails.RemoveRange(toDelete.Emails);
                    context.EmailTemplates.Remove(toDelete);
                }

                context.SaveChanges();
            }
        }


        // gets emails by entity
        public List<Email> GetEmailsByEntity(EntityTypeEnum entityTypeId, long entityId, EmailIncludes includes = EmailIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                context.Database.CommandTimeout = EmailSelectTimeout;

                var result = context.Emails.AddIncludes(includes).Active()
                    .Where(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId && !x.IsTestEmail)
                    .OrderBy(x => x.Created).ToList();                               
                return result;
            }
        }

        #endregion


        #region email service

        // gets all email parameters
        public List<EmailParameter> GetEmailParameters()
        {
            using (var context = new CoreEntities())
            {
                var result = context.EmailParameters.Active().ToList();
                return result;
            }
        }

        // saves email
        public long SaveEmail(Email email)
        {
            using (var context = new CoreEntities())
            {
                context.Emails.Add(email);
                context.SaveChanges();
                return email.EmailId;
            }
        }

        // gets email by id
        public Email GetEmailById(long emailId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Emails.Include("EmailAttachments").Active().FirstOrDefault(x => x.EmailId == emailId);
                if (result != null)
                    result.EmailAttachments = result.EmailAttachments.AsQueryable().Active().ToList();
                return result;
            }
        }

        // updates email
        public void UpdateEmail(long emailId, bool isSent, DateTime? sent, int sendingAttempts)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Emails.Active().First(x => x.EmailId == emailId);
                result.IsSent = isSent;
                result.Sent = sent;
                result.SendingAttempts = sendingAttempts;
                context.SaveChanges();                
            }
        }

        #endregion

    }

}
