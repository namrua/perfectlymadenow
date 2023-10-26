using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.Home.AppLogic
{
    public class WwaRegistrationSplitter : IWwaRegistrationSplitter
    {
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public WwaRegistrationSplitter(IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public List<ClassPublicDetail> SplitWwaClasses(List<ClassPublicDetail> classes)
        {
            var result = new List<ClassPublicDetail>();

            foreach (var detail in classes)
            {
                result.Add(detail);
                if (CanApplyToClass(detail.ClassCategoryId, detail.IsWwaFormAllowed))
                {
                    var wwaDetail = detail.Clone();
                    wwaDetail.MarkedAsWwa = true;
                    result.Add(wwaDetail);
                }
            }

            return result;
        }

        public List<RegistrationTypeListItem> FilterRegistrationTypes(List<RegistrationTypeListItem> types, bool? forWwa)
        {
            var result = new List<RegistrationTypeListItem>(types);
            if (!forWwa.HasValue)
            {
                return result;
            }

            result = forWwa.Value
                ? types.Where(x => registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList()
                : types.Where(x => !registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList();
            
            return result;
        }

        public bool? ResolveForWwa(bool? forWwa, RegistrationTypeEnum? backFromRegistrationTypeId, ClassCategoryEnum classCategoryId, bool isWwaAllowed)
        {
            if (!CanApplyToClass(classCategoryId, isWwaAllowed))
            {
                return null;
            }

            var isBackFromRegistrationTypeIdWwa = backFromRegistrationTypeId.HasValue
                ? (bool?) registrationTypeResolver.IsWwaRegistration(backFromRegistrationTypeId.Value)
                : null;

            return forWwa ?? isBackFromRegistrationTypeIdWwa;
        }

        #region private methods

        private bool CanApplyToClass(ClassCategoryEnum classCategoryId, bool isWwaAllowed)
        {
            return classCategoryId != ClassCategoryEnum.DistanceClass && isWwaAllowed;
        }

        #endregion
    }
}
