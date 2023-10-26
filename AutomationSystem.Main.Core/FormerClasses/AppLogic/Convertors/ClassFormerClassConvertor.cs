using System;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors
{

    /// <summary>
    /// Converts class and registration to former class and student
    /// </summary>
    public class ClassFormerClassConvertor : IClassFormerClassConvertor
    {

        private readonly IAddressConvertor addressConvertor;


        // constructor
        public ClassFormerClassConvertor(IAddressConvertor addressConvertor)
        {
            this.addressConvertor = addressConvertor;
        }

        // converts Class to Former class
        public FormerClass ConvertToFormerClass(Class cls, int ownerId)
        {
            var result = new FormerClass
            {              
                ClassTypeId = cls.ClassTypeId,
                Location = cls.Location,
                EventStart = cls.EventStart,
                EventEnd = cls.EventEnd,
                IsTemporary = false,
                OwnerId = ownerId,
                ProfileId = cls.ProfileId
            };
            return result;
        }

        // converts Class registration to Former student
        public FormerStudent ConvertToFormerStudent(ClassRegistration registration, int ownerId)
        {
            if (registration.StudentAddress == null)
                throw new InvalidOperationException("StudentAddress is not included into ClassRegistration object.");

            var result = new FormerStudent
            {
                Address = addressConvertor.CloneAddress(registration.StudentAddress, false),
                Email = registration.StudentEmail,
                Phone = registration.StudentPhone,
                IsTemporary = false,
                OwnerId = ownerId
            };
            return result;
        }

    }   

}
