using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Shared.Contract.Enums.Data;
using System.Linq;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateFactory : IDistanceClassTemplateFactory
    {
        private readonly IPersonDatabaseLayer personDb;
        private readonly IEnumDatabaseLayer enumDb;
        private readonly ILanguageTranslationProvider translationProvider;
        private readonly IClassTypeResolver classTypeResolver;

        public DistanceClassTemplateFactory(
            IPersonDatabaseLayer personDb,
            IEnumDatabaseLayer enumDb,
            ILanguageTranslationProvider translationProvider,
            IClassTypeResolver classTypeResolver)
        {
            this.personDb = personDb;
            this.enumDb = enumDb;
            this.translationProvider = translationProvider;
            this.classTypeResolver = classTypeResolver;
        }

        public DistanceClassTemplateForEdit CreateDistanceClassTemplateForEdit()
        {
            var allowedClassTypes = classTypeResolver.GetClassTypesByClassCategoryId(ClassCategoryEnum.DistanceClass);
            var classTypes = enumDb.GetItemsByFilter(EnumTypeEnum.MainClassType)
                .Where(x => allowedClassTypes.Contains((ClassTypeEnum)x.Id)).ToList();
            var persons = personDb.GetMinimizedPersonsByProfileId(null);
            var translations = translationProvider.GetTranslationOptions();
            var forEdit = new DistanceClassTemplateForEdit
            {
                ClassTypes = classTypes,
                Persons = new PersonHelper(persons),
                Translations = translations
            };
            return forEdit;
        }
    }
}
