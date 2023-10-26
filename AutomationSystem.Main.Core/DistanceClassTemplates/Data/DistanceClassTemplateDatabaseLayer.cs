using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System;
using PerfectlyMadeInc.Helpers.Database;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.Data
{
    public class DistanceClassTemplateDatabaseLayer : IDistanceClassTemplateDatabaseLayer
    {
        public List<DistanceClassTemplate> GetDistanceClassTemplatesByFilter(DistanceClassTemplateFilter filter = null, DistanceClassTemplateIncludes includes = DistanceClassTemplateIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceClassTemplates.AddIncludes(includes).Filter(filter).ToList();
                result = result.Select(x => DistanceClassTemplateRemoveInactive.RemoveInactiveForDistanceClassTemplate(x, includes)).ToList();
                return result;
            }
        }

        public DistanceClassTemplate GetDistanceClassTemplateById(long id, DistanceClassTemplateIncludes includes = DistanceClassTemplateIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.DistanceClassTemplates.AddIncludes(includes).Active().FirstOrDefault(x => x.DistanceClassTemplateId == id);
                result = DistanceClassTemplateRemoveInactive.RemoveInactiveForDistanceClassTemplate(result, includes);
                return result;
            }
        }

        public bool PersonOnAnyDistanceClassTemplate(long personId)
        {
            using (var context = new MainEntities())
            {
                var anyDistanceClassTemplate = context.DistanceClassTemplates.Active().Any(x => x.GuestInstructorId == personId);
                if (anyDistanceClassTemplate)
                {
                    return true;
                }

                var anyDistanceClassTemplatePerson = context.DistanceClassTemplatePersons.ActiveInActiveDistanceClassTemplate().Any(x => x.PersonId == personId);
                if (anyDistanceClassTemplatePerson)
                {
                    return true;
                }

                return false;
            }
        }

        public long InsertDistanceClassTemplate(DistanceClassTemplate template)
        {
            using (var context = new MainEntities())
            {
                context.DistanceClassTemplates.Add(template);
                context.SaveChanges();
                return template.DistanceClassTemplateId;
            }
        }

        public void UpdateDistanceClassTemplate(DistanceClassTemplate template)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.DistanceClassTemplates.FirstOrDefault(x => x.DistanceClassTemplateId == template.DistanceClassTemplateId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no distance class with id {template.DistanceClassTemplateId}.");
                }

                toUpdate.ClassTypeId = template.ClassTypeId;
                toUpdate.EventStart = template.EventStart;
                toUpdate.EventEnd = template.EventEnd;
                toUpdate.GuestInstructorId = template.GuestInstructorId;
                toUpdate.Location = template.Location;
                toUpdate.OriginLanguageId = template.OriginLanguageId;
                toUpdate.TransLanguageId = template.TransLanguageId;
                toUpdate.RegistrationStart = template.RegistrationStart;
                toUpdate.RegistrationEnd = template.RegistrationEnd;
                
                var persons = context.DistanceClassTemplatePersons.Active()
                    .Where(x => x.DistanceClassTemplateId == template.DistanceClassTemplateId && x.RoleTypeId == PersonRoleTypeEnum.Instructor)
                    .ToList();
                var updateResolver = new SetUpdateResolver<DistanceClassTemplatePerson, long>(
                    x => x.PersonId,
                    (origItem, newItem) => { });
                var strategy = updateResolver.ResolveStrategy(persons, template.DistanceClassTemplatePersons);
                context.DistanceClassTemplatePersons.AddRange(strategy.ToAdd);
                context.DistanceClassTemplatePersons.RemoveRange(strategy.ToDelete);

                context.SaveChanges();
            }
        }

        public void ApproveDistanceClassTemplate(long id)
        {
            using (var context = new MainEntities())
            {
                var toApprove = context.DistanceClassTemplates.Active().FirstOrDefault(x => x.DistanceClassTemplateId == id);
                if (toApprove == null)
                {
                    throw new ArgumentException($"There is no distance class template with id {id}.");
                }

                if (toApprove.IsApproved)
                {
                    throw new InvalidOperationException($"Distance class template with id {id} is already approved.");
                }

                toApprove.IsApproved = true;
                toApprove.Approved = DateTime.Now;
                context.SaveChanges();
            }
        }

        public void CompleteDistanceClassTemplate(long id)
        {
            using (var context = new MainEntities())
            {
                var toComplete = context.DistanceClassTemplates.Active().FirstOrDefault(x => x.DistanceClassTemplateId == id);
                if (toComplete == null)
                {
                    throw new ArgumentException($"There is no distance class template with id {id}.");
                }

                if (toComplete.IsCompleted)
                {
                    throw new InvalidOperationException($"Distance class template with id {id} is already completed.");
                }

                toComplete.IsCompleted = true;
                toComplete.Completed = DateTime.Now;
                context.SaveChanges();
            }
        }

        public void DeleteDistanceClassTemplate(long id)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.DistanceClassTemplates.Active().FirstOrDefault(x => x.DistanceClassTemplateId == id);
                if (toDelete == null)
                {
                    return;
                }

                context.DistanceClassTemplates.Remove(toDelete);
                context.SaveChanges();
            }
        }

        public void UpdateDistanceClassTemplateCompletionSettings(long id, DateTime? automationTimeCompletion)
        {
            using (var context = new MainEntities())
            {
                var template = context.DistanceClassTemplates.Active().FirstOrDefault(x => x.DistanceClassTemplateId == id);
                if (template == null)
                {
                    throw new ArgumentException($"There is no distance class with id {template.DistanceClassTemplateId}.");
                }

                template.AutomationCompleteTime = automationTimeCompletion;

                context.SaveChanges();
            }
        }
    }
}
