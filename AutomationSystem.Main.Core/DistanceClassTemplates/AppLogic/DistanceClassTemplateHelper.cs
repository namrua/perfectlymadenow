using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Routines;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Persons.AppLogic;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateHelper : IDistanceClassTemplateHelper
    {
        public HashSet<long> GetDistanceClassTemplatePersonIds(DistanceClassTemplate template)
        {
            EntityHelper.CheckForNull(template.DistanceClassTemplatePersons, "DistanceClassTemplatePersons", "DistanceClassTemplate");

            var result = new HashSet<long>();
            if (template.GuestInstructorId.HasValue)
            {
                result.Add(template.GuestInstructorId.Value);
            }

            result.UnionWith(template.DistanceClassTemplatePersons.Select(x => x.PersonId));
            return result;
        }

        public HashSet<long> GetDistanceClassTemplatePersonsIds(List<DistanceClassTemplate> templates)
        {
            var result = new HashSet<long>();
            foreach (var template in templates)
            {
                var personIds = GetDistanceClassTemplatePersonIds(template);
                result.UnionWith(personIds);
            }
            return result;
        }

        public List<string> GetDistanceClassTemplateInstructorsWithGuestInstructor(DistanceClassTemplate template, IPersonHelper personHelper)
        {
            EntityHelper.CheckForNull(template.DistanceClassTemplatePersons, "DistanceClassTemplatePersons", "DistanceClassTemplate");

            var instructorComposer = new PersonDistinctComposer<string>(x => personHelper.GetPersonNameById(x));
            instructorComposer.AddPerson(template.GuestInstructorId);
            instructorComposer.AddDistanceClassTemplatePersonWithRoles(template.DistanceClassTemplatePersons, PersonRoleTypeEnum.Instructor);
            return instructorComposer.Pop();    
        }

        public List<string> GetDistanceClassTemplateInstructors(DistanceClassTemplate template, IPersonHelper personHelper)
        {
            EntityHelper.CheckForNull(template.DistanceClassTemplatePersons, "DistanceClassTemplatePersons", "DistanceClassTemplate");

            var instructorComposer = new PersonDistinctComposer<string>(x => personHelper.GetPersonNameById(x));
            instructorComposer.AddDistanceClassTemplatePersonWithRoles(template.DistanceClassTemplatePersons, PersonRoleTypeEnum.Instructor);
            return instructorComposer.Pop();
        }

        public DistanceClassTemplateState GetDistanceClassTemplateState(DistanceClassTemplate template)
        {
            if (template.IsCompleted)
            {
                return DistanceClassTemplateState.Completed;
            }

            if (template.IsApproved)
            {
                return DistanceClassTemplateState.Approved;
            }

            return DistanceClassTemplateState.New;
        }
    }
}
