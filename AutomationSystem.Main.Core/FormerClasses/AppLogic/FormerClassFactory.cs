using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Shared.Contract.Enums.Data;
using CorabeuControl.Components;
using System.Linq;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{
    public class FormerClassFactory : IFormerClassFactory
    {
        private readonly IEnumDatabaseLayer enumDb;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IClassTypeResolver classTypeResolver;

        public FormerClassFactory(IEnumDatabaseLayer enumDb, IProfileDatabaseLayer profileDb, IClassTypeResolver classTypeResolver)
        {
            this.enumDb = enumDb;
            this.profileDb = profileDb;
            this.classTypeResolver = classTypeResolver;
        }

        public FormerClassForEdit CreateFormerClassForEdit()
        {
            var allowedClassTypes = classTypeResolver.GetAllowedClassTypesForFormerClasses();
            var profiles = profileDb.GetProfilesByFilter();
            var result = new FormerClassForEdit
            {
                ClassTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainClassType)
                    .FilterByIdSet(allowedClassTypes, x => (ClassTypeEnum)x).ToList(),
                Profiles = profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)).ToList()
            };

            return result;
        }
    }
}
