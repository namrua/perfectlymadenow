using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using System;

namespace AutomationSystem.Main.Core.Emails.System
{
    public class MainEmailHelper : IMainEmailHelper
    {
        private readonly IPersonDatabaseLayer personDb;

        public MainEmailHelper(IPersonDatabaseLayer personDb)
        {
            this.personDb = personDb;
        }

        public SenderInfo GetSenderInfoByPersonId(long? personId)
        {
            if (personId == null)
                return new SenderInfo();
            var person = personDb.GetPersonById(personId.Value, PersonIncludes.Address);
            if (person == null)
                throw new ArgumentException($"There is no Person with id {personId.Value}.");
            var result = new SenderInfo()
            {
                SenderEmail = person.Email,
                SenderName = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName)
            };
            return result;
        }

        public string GetRegistrationRecipientEmail(ClassRegistration registration, RecipientType type)
        {
            string result = null;
            switch (type)
            {
                // normal student
                case RecipientType.Student:
                    result = registration.RegistrantEmail ?? registration.StudentEmail;
                    break;

                // class coordinator
                case RecipientType.Coordinator:
                    var coordinator = personDb.GetPersonById(registration.Class.CoordinatorId);
                    if (coordinator != null)
                        result = coordinator.Email;
                    break;
            }

            if (result == null)
            {
                throw new InvalidOperationException(
                    $"Cannot obtain recipient's email by type {type} from Class registration with id {registration.ClassRegistrationId}.");
            }

            return result;
        }
    }
}
