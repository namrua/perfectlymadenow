using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Model;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{

    /// <summary>
    /// Propagates class and class registration into former database
    /// </summary>
    public class FormerClassPropagator : IFormerClassPropagator
    {
        
        private readonly IFormerDatabaseLayer formerDb;
        private readonly IClassFormerClassConvertor convertor;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IIdentityResolver identityResolver;
        private readonly IClassTypeResolver classTypeResolver;


        // constructor
        public FormerClassPropagator(
            IFormerDatabaseLayer formerDb,
            IClassFormerClassConvertor convertor,
            IRegistrationTypeResolver registrationTypeResolver,
            IIdentityResolver identityResolver,
            IClassTypeResolver classTypeResolver)
        {           
            this.formerDb = formerDb;
            this.convertor = convertor;
            this.registrationTypeResolver = registrationTypeResolver;
            this.identityResolver = identityResolver;
            this.classTypeResolver = classTypeResolver;
        }


        // propagates class and class registrations to former database
        public long? PropagateToFormerDatabase(Class cls, List<ClassRegistration> classRegistrations)
        {
            var ownerId = identityResolver.GetOwnerId();

            if (!classTypeResolver.IsPropagationToFormerClassesAllowed(cls.ClassCategoryId))
            {
                return null;
            }

            var registrationForPropagation = classRegistrations.Where(x => !registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList();

            // converts entities to former entities 
            var formerClass = convertor.ConvertToFormerClass(cls, ownerId);
            formerClass.FormerStudents = registrationForPropagation.Select(x => convertor.ConvertToFormerStudent(x, ownerId)).ToList();
            
            // save former class
            var result = formerDb.InsertFormerClass(formerClass);
            return result;
        }

    }   

}
