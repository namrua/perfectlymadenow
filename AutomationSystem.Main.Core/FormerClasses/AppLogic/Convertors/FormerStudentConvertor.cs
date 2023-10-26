using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors
{

    /// <summary>
    /// Converts former student objects
    /// </summary>
    public class FormerStudentConvertor : IFormerStudentConvertor
    {

        // private components
        private readonly IAddressConvertor addressConvertor;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IMainMapper mainMapper;
        

        // constructor
        public FormerStudentConvertor(
            IEnumDatabaseLayer enumDb,
            IAddressConvertor addressConvertor,
            IMainMapper mainMapper)
        {
            this.enumDb = enumDb;
            this.addressConvertor = addressConvertor;
            this.mainMapper = mainMapper;
        }


        // initializes FormerStudentForEdit
        public FormerStudentForEdit InitializeFormerStudentForEdit()
        {
            var result = new FormerStudentForEdit
            {
                Countries = enumDb.GetItemsByFilter(EnumTypeEnum.Country).SortDefaultCountryFirst()
            };
            return result;
        }

        // converts former student to former student detail
        public FormerStudentDetail ConvertToFormerStudentDetail(FormerStudent formerStudent, bool includeFormerClass = false)
        {
            var result = new FormerStudentDetail
            {
                FormerStudentId = formerStudent.FormerStudentId,
                Email = formerStudent.Email,
                Phone = formerStudent.Phone,
                Address = addressConvertor.ConvertToAddressDetail(formerStudent.Address)
            };

            if (includeFormerClass)
            {
                result.Class = mainMapper.Map<FormerClassDetail>(formerStudent.FormerClass);
            }

            return result;
        }

        // converts former student to fs form
        public FormerStudentForm ConvertToFormerStudentForm(FormerStudent formerStudent)
        {
            var result = new FormerStudentForm
            {
                FormerStudentId = formerStudent.FormerStudentId,
                FormerClassId = formerStudent.FormerClassId,
                Email = formerStudent.Email,
                Phone = formerStudent.Phone,
                Address = addressConvertor.ConvertToAddressForm(formerStudent.Address),
            };
            return result;
        }

        // converts form former student to db former student
        public FormerStudent ConvertToFormerStudent(FormerStudentForm form, bool isTemporary)
        {
            var result = new FormerStudent
            {
                FormerStudentId = form.FormerStudentId,
                FormerClassId = form.FormerClassId,
                Email = form.Email,
                Phone = form.Phone,
                IsTemporary = isTemporary,
                Address = addressConvertor.ConvertToAddress(form.Address, false)
            };
            return result;
        }

        public FormerStudentListItem ConvertToFormerStudentListItem(FormerStudent formerStudent, bool includeFormerClass = false)
        {
            var result = new FormerStudentListItem
            {
                FormerStudentId = formerStudent.FormerStudentId,
                Email = formerStudent.Email,
                Phone = formerStudent.Phone,
                Address = addressConvertor.ConvertToAddressDetail(formerStudent.Address)
            };

            if (includeFormerClass)
            {
                result.Class = mainMapper.Map<FormerClassListItem>(formerStudent.FormerClass);
            }
                
            return result;
        }

    }

}
