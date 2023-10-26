using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Console.ConsoleCommands.Models;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Core.Emails.Data.Extensions;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Console.ConsoleCommands
{
    public class EmailMigrationCommand : IConsoleCommand<EmailMigrationParameters>
    {
        public void Execute(EmailMigrationParameters commandParameters)
        {
            List<EmailTemplate> sourceTemplates;
            Dictionary<string, long> sourceParameterMap;
            using (var sourceCtx = new CoreEntities($"name={commandParameters.SourceDatabaseName}"))
            {
                sourceTemplates = sourceCtx.EmailTemplates
                    .AddIncludes(EmailTemplateIncludes.EmailTemplateParameter)
                    .Active()
                    .Where(x => !x.EntityId.HasValue && !x.EntityTypeId.HasValue && !x.IsDefault).ToList();

                sourceParameterMap = sourceCtx.EmailParameters.Active().ToDictionary(x => x.Name, y => y.EmailParameterId);
            }

            using (var destinationCtx = new CoreEntities($"name={commandParameters.DestinationDatabaseName}"))
            {
                var sourceToDestinationParameterIdMap = destinationCtx.EmailParameters
                    .Active()
                    .ToList()
                    .Where(x => sourceParameterMap.ContainsKey(x.Name))
                    .ToDictionary(x => sourceParameterMap[x.Name], y => y.EmailParameterId);
                var templatesToUpdate = destinationCtx.EmailTemplates
                    .AddIncludes(EmailTemplateIncludes.EmailTemplateParameter)
                    .Active()
                    .Where(x => !x.EntityId.HasValue && !x.EntityTypeId.HasValue && !x.IsDefault).ToList();

                foreach (var sourceTemplate in sourceTemplates)
                {
                    var templateToUpdate = templatesToUpdate.FirstOrDefault(x => x.LanguageId == sourceTemplate.LanguageId && x.EmailTypeId == sourceTemplate.EmailTypeId);

                    if (templateToUpdate == null)
                    {
                        var newTemplate = new EmailTemplate
                        {
                            LanguageId = sourceTemplate.LanguageId,
                            EmailTypeId = sourceTemplate.EmailTypeId,
                            IsDefault = sourceTemplate.IsDefault,
                            IsLocalisable = sourceTemplate.IsLocalisable,
                            IsSealed = sourceTemplate.IsSealed,
                            EntityTypeId = sourceTemplate.EntityTypeId,
                            EntityId = sourceTemplate.EntityId,
                            Subject = sourceTemplate.Subject,
                            Text = sourceTemplate.Text,
                            FillingNote = sourceTemplate.FillingNote,
                            IsValidated = sourceTemplate.IsValidated,
                            IsHtml = sourceTemplate.IsHtml
                        };

                        foreach (var sourceParam in sourceTemplate.EmailTemplateParameters)
                        {
                            var newParam = new EmailTemplateParameter
                            {
                                EmailParameterId = sourceToDestinationParameterIdMap[sourceParam.EmailParameterId],
                                IsRequired = sourceParam.IsRequired
                            };
                            newTemplate.EmailTemplateParameters.Add(newParam);
                        }

                        destinationCtx.SaveChanges();
                    }
                    else
                    {
                        templateToUpdate.Text = sourceTemplate.Text;
                        templateToUpdate.FillingNote = sourceTemplate.FillingNote;
                        templateToUpdate.Subject = sourceTemplate.Subject;
                        templateToUpdate.IsValidated = sourceTemplate.IsValidated;

                        foreach (var sourceParam in sourceTemplate.EmailTemplateParameters)
                        {
                            var destinationParameterId = sourceToDestinationParameterIdMap[sourceParam.EmailParameterId];
                            var paramToUpdate = templateToUpdate.EmailTemplateParameters.First(x => x.EmailParameterId == destinationParameterId);

                            paramToUpdate.IsRequired = sourceParam.IsRequired;

                        }

                        destinationCtx.SaveChanges();
                    }
                }
            }
        }
    }
}
