using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using System;
using System.Linq;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{

    /// <summary>
    /// Service for former students and classes administration
    /// </summary>
    public class FormerClassAdministration : IFormerClassAdministration
    {

        // private components
        private readonly IFormerDatabaseLayer formerDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IFormerStudentConvertor studentConvertor;
        private readonly IFormerClassFactory formerClassFactory;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainMapper mainMapper;
        private readonly IClassTypeResolver classTypeResolver;


        // constructor
        public FormerClassAdministration(
            IFormerDatabaseLayer formerDb,
            IEnumDatabaseLayer enumDb,
            IIdentityResolver identityResolver,
            IFormerClassFactory formerClassFactory,
            IFormerStudentConvertor studentConvertor,
            IMainMapper mainMapper,
            IClassTypeResolver classTypeResolver)
        {
            this.formerDb = formerDb;
            this.enumDb = enumDb;
            this.formerClassFactory = formerClassFactory;
            this.identityResolver = identityResolver;
            this.studentConvertor = studentConvertor;
            this.mainMapper = mainMapper;
            this.classTypeResolver = classTypeResolver;
        }
        

        // gets former classes for list
        public FormerClassesForList GetFormerClassesForList(FormerClassFilter filter, bool search)
        {
            var allowedClassTypesForFiltering = classTypeResolver.GetAllowedClassTypesForFormerClassFiltering();

            // normalizes classtype and fill synonymous class types
            if (filter?.ClassTypeId != null)
            {
                filter.ClassTypeId = classTypeResolver.NormalizeClassTypeSynonymForFormerClassFiltering(filter.ClassTypeId);
                filter.SynonymousClassTypes = classTypeResolver.GetSynonymousClassTypesForFormerClassFiltering(filter.ClassTypeId);
            }

            // assembles model
            var result = new FormerClassesForList(filter);
            result.ClassTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainClassType)
                .FilterByIdSet(allowedClassTypesForFiltering, x => (ClassTypeEnum)x).ToList();
            result.WasSearched = search;
            result.CanInsert = identityResolver.IsEntitleGranted(Entitle.MainFormerClasses);
            if (search)
            {
                var classes = formerDb.GetFormerClassesByFilter(filter, FormerClassIncludes.ClassType);
                result.Items = classes.Select(mainMapper.Map<FormerClassListItem>).ToList();
            }
            return result;
        }

        // gets former class detail by id
        public FormerClassDetail GetFormerClassById(long formerClassId)
        {
            var formerClass = formerDb.GetFormerClassById(formerClassId, FormerClassIncludes.ClassType | FormerClassIncludes.Profile);
            if (formerClass == null)
                throw new ArgumentException($"There is no Former class with id {formerClassId}.");

            var result = mainMapper.Map<FormerClassDetail>(formerClass);
            result.CanEdit = result.CanDelete = identityResolver.IsEntitleGranted(Entitle.MainFormerClasses);
            return result;// gets former class students list            
        }


        // gets former class students 
        public FormerClassStudents GetFormerClassStudents(long formerClassId)
        {
            // loads class
            var formerClass = formerDb.GetFormerClassById(formerClassId, FormerClassIncludes.ClassType);
            if (formerClass == null)
                throw new ArgumentException($"There is no Former class with id {formerClassId}.");

            // loads students
            var students = formerDb.GetFormerStudentsByFilter(
                new FormerStudentFilter { FormerClassId = formerClassId },
                FormerStudentIncludes.AddressCountry | FormerStudentIncludes.FormerClassClassType);

            // assembles result
            var result = new FormerClassStudents();
            result.Class = mainMapper.Map<FormerClassListItem>(formerClass);
            result.Students = students.Select(x => studentConvertor.ConvertToFormerStudentListItem(x, true)).ToList();
            result.CanInsert = identityResolver.IsEntitleGranted(Entitle.MainFormerClasses);
            return result;
        }

        // gets new former class for edit
        public FormerClassForEdit GetNewFormerClassForEdit()
        {
            var result = formerClassFactory.CreateFormerClassForEdit();
            return result;
        }

        // gets former class for edit by fc id
        public FormerClassForEdit GetFormerClassForEdit(long formerClassId)
        {
            var formerClass = formerDb.GetFormerClassById(formerClassId, FormerClassIncludes.ClassType);
            if (formerClass == null)
                throw new ArgumentException($"There is no Former class with id {formerClassId}.");

            var result = formerClassFactory.CreateFormerClassForEdit();
            result.Form = mainMapper.Map<FormerClassForm>(formerClass);
            return result;
        }

        // gets former class for edit based on form
        public FormerClassForEdit GetFormFormerClassForEdit(FormerClassForm form)
        {
            var result = formerClassFactory.CreateFormerClassForEdit();
            result.Form = form;
            return result;
        }

        // validates 
        public bool ValidateFormerClassForm(FormerClassForm form)
        {
            // checks whether event start is lower that event end
            if (form.EventStart.HasValue && form.EventEnd.HasValue && form.EventStart > form.EventEnd)
                return false;
            return true;
        }


        // saves former class
        public long SaveFormerClass(FormerClassForm form)
        {
            var dbClass = mainMapper.Map<FormerClass>(form);
            dbClass.IsTemporary = false;
            var result = form.FormerClassId;
            if (result == 0)
            {
                //todo #OWNER
                dbClass.OwnerId = 1;
                result = formerDb.InsertFormerClass(dbClass);
            }
            else
            {
                formerDb.UpdateFormerClass(dbClass, FormerOperationOption.KeepOwnerId);
            }
            return result;
        }

        // deletes former class
        public void DeleteFormerClass(long formerClassId)
        {
            formerDb.DeleteFormerClass(formerClassId, FormerOperationOption.CheckStudentNotUsedForReview);
        }
    }
}