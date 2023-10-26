using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Shared.Contract.Enums.Data;
using System;
using System.Linq;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{

    /// <summary>
    /// Service for former students and classes administration
    /// </summary>
    public class FormerStudentAdministration : IFormerStudentAdministration
    {

        // private components
        private readonly IFormerDatabaseLayer formerDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IFormerStudentConvertor studentConvertor;
        private readonly IIdentityResolver identityResolver;
        private readonly IClassTypeResolver classTypeResolver;


        // constructor
        public FormerStudentAdministration(
            IFormerDatabaseLayer formerDb,
            IEnumDatabaseLayer enumDb,
            IIdentityResolver identityResolver,
            IFormerStudentConvertor studentConvertor,
            IClassTypeResolver classTypeResolver)
        {
            this.formerDb = formerDb;
            this.enumDb = enumDb;
            this.identityResolver = identityResolver;
            this.studentConvertor = studentConvertor;
            this.classTypeResolver = classTypeResolver;
        }

        // gets former students for list
        public FormerStudentsForList GetFormerStudentsForList(FormerStudentFilter filter, bool search)
        {
            var allowedClassTypesForFiltering = classTypeResolver.GetAllowedClassTypesForFormerClassFiltering();

            // normalizes classtype and fill synonymous class types
            if (filter?.Class?.ClassTypeId != null)
            {
                filter.Class.ClassTypeId = classTypeResolver.NormalizeClassTypeSynonymForFormerClassFiltering(filter.Class.ClassTypeId);
                filter.Class.SynonymousClassTypes = classTypeResolver.GetSynonymousClassTypesForFormerClassFiltering(filter.Class.ClassTypeId);
            }

            // assembles model
            var result = new FormerStudentsForList(filter);
            result.ClassTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainClassType)
                .FilterByIdSet(allowedClassTypesForFiltering, x => (ClassTypeEnum)x).ToList(); 
            result.Countries = enumDb.GetItemsByFilter(EnumTypeEnum.Country);
            result.WasSearched = search;
            if (search)
            {
                var students = formerDb.GetFormerStudentsByFilter(filter, 
                    FormerStudentIncludes.AddressCountry | FormerStudentIncludes.FormerClassClassType);
                result.Items = students.Select(x => studentConvertor.ConvertToFormerStudentListItem(x, true)).ToList();
            }
            return result;
        }      

        // gets student detail by id 
        public FormerStudentDetail GetFormerStudentById(long formerStudentId)
        {
            var student = formerDb.GetFormerStudentById(formerStudentId, 
                FormerStudentIncludes.AddressCountry | FormerStudentIncludes.FormerClassClassType | FormerStudentIncludes.FormerClassProfile);
            if (student == null)
                throw new ArgumentException($"There is no Former student with id {formerStudentId}.");

            var result = studentConvertor.ConvertToFormerStudentDetail(student, true);
            result.CanEdit = result.CanDelete = identityResolver.IsEntitleGranted(Entitle.MainFormerClasses);
            return result;
        }


        // gets new former student for edit
        public FormerStudentForEdit GetNewFormerStudentForEdit(long formerClassId)
        {
            // check whether format class exists in the database
            var formerClass = formerDb.GetFormerClassById(formerClassId);
            if (formerClass == null)
                throw new ArgumentException($"There is no Former class with id {formerClassId}.");

            // assembles result
            var result = studentConvertor.InitializeFormerStudentForEdit();                           
            result.Form.FormerClassId = formerClassId;
            return result;
        }

        // gets former student for edit by fs id
        public FormerStudentForEdit GetFormerStudentForEdit(long formerStudentId)
        {
            // gets former student
            var student = formerDb.GetFormerStudentById(formerStudentId, FormerStudentIncludes.Address);
            if (student == null)
                throw new ArgumentException($"There is no Former student with id {formerStudentId}.");

            // creates result            
            var result = studentConvertor.InitializeFormerStudentForEdit();
            result.Form = studentConvertor.ConvertToFormerStudentForm(student);
            return result;
        }

        // gets former student for edit based on form
        public FormerStudentForEdit GetFormFormerStudentForEdit(FormerStudentForm form)
        {
            var result = studentConvertor.InitializeFormerStudentForEdit();
            result.Form = form;
            return result;
        }


        // saves former student
        public long SaveFormerStudent(FormerStudentForm form)
        {
            var dbFormerStudent = studentConvertor.ConvertToFormerStudent(form, false);
            var result = form.FormerStudentId;
            if (result == 0)
            {
                dbFormerStudent.OwnerId = identityResolver.GetOwnerId();
                result = formerDb.InsertFormerStudent(dbFormerStudent);
            }
            else
            {
                formerDb.UpdateFormerStudent(dbFormerStudent, FormerOperationOption.KeepOwnerId | FormerOperationOption.CheckStudentClassConsistency);
            }

            return result;
        }

       
        // deletes former student 
        public void DeleteFormerStudent(long formerStudentId)
        {
            formerDb.DeleteFormerStudent(formerStudentId, FormerOperationOption.CheckStudentNotUsedForReview);
        }
        
    }

}
