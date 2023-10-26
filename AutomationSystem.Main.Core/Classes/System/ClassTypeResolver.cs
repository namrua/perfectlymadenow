using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Model;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.System
{
    /// <summary>
    /// Resolves ClassType relevant logic and business rules
    /// </summary>
    public class ClassTypeResolver : IClassTypeResolver
    {
        private readonly List<ClassTypeInfo> types;
        private readonly Dictionary<ClassTypeEnum, ClassTypeInfo> typeMap;
        private readonly Dictionary<ClassCategoryEnum, ClassFeaturesConfiguration> classFormConfigurationMap;

        public ClassTypeResolver()
        {
            types = new List<ClassTypeInfo>(new []
            {
                new ClassTypeInfo(ClassTypeEnum.BasicOnline, ClassTypeCategory.Class, ClassTypeTopic.Basic, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.BusinessOnline, ClassTypeCategory.Class, ClassTypeTopic.Business, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.HealthOnline, ClassTypeCategory.Class, ClassTypeTopic.Health, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.Basic2Online, ClassTypeCategory.Class, ClassTypeTopic.Basic2, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.Basic, ClassTypeCategory.Class, ClassTypeTopic.Basic, ClassTypeShape.InPerson),
                new ClassTypeInfo(ClassTypeEnum.Business, ClassTypeCategory.Class, ClassTypeTopic.Business, ClassTypeShape.InPerson),
                new ClassTypeInfo(ClassTypeEnum.Health, ClassTypeCategory.Class, ClassTypeTopic.Health, ClassTypeShape.InPerson),
                new ClassTypeInfo(ClassTypeEnum.Basic2, ClassTypeCategory.Class, ClassTypeTopic.Basic2, ClassTypeShape.InPerson),

                new ClassTypeInfo(ClassTypeEnum.LectureBasicOnline, ClassTypeCategory.Lecture, ClassTypeTopic.Basic, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.LectureBusinessOnline, ClassTypeCategory.Lecture, ClassTypeTopic.Business, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.LectureHealthOnline, ClassTypeCategory.Lecture, ClassTypeTopic.Health, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.LectureBasic2Online, ClassTypeCategory.Lecture, ClassTypeTopic.Basic2, ClassTypeShape.Online),
                new ClassTypeInfo(ClassTypeEnum.LectureBasic, ClassTypeCategory.Lecture, ClassTypeTopic.Basic, ClassTypeShape.InPerson),
                new ClassTypeInfo(ClassTypeEnum.LectureBusiness, ClassTypeCategory.Lecture, ClassTypeTopic.Business, ClassTypeShape.InPerson),
                new ClassTypeInfo(ClassTypeEnum.LectureHealth, ClassTypeCategory.Lecture, ClassTypeTopic.Health, ClassTypeShape.InPerson),
                new ClassTypeInfo(ClassTypeEnum.LectureBasic2, ClassTypeCategory.Lecture, ClassTypeTopic.Basic2, ClassTypeShape.InPerson),
            });
            typeMap = types.ToDictionary(x => x.Id);

            classFormConfigurationMap = new Dictionary<ClassCategoryEnum, ClassFeaturesConfiguration>
            {
                [ClassCategoryEnum.Class] = new ClassFeaturesConfiguration
                {
                    ClassFormConfiguration = new ClassFormConfiguration
                    {
                        IsWwaAllowedValue = null,
                        ShowOnlyDates = false,
                        UseDistanceCoordinator = false,
                        ShowApprovedStaffIds = true,
                        ShowIntegrationCode = true,
                        ShowPayPalKey = true
                    },
                    AreAutomationNotificationsAllowed = true,
                    AreStyleAndBehaviorAllowed = true,
                    ShowClassBehaviorSettings = true,
                    AreInvitationsAllowed = true,
                    AreCertificatesAllowed = true,
                    AreCertificatesAllowedForClassPersons = true,
                    AreReportsAllowed = true,
                    AreFinancialFormsAllowed = true,
                    IsSupervisedByMasterCoordinator = true,
                    IsPropagationToFormerClassesAllowed = true,
                    IsCurrencyAllowed = true,
                    AreMaterialsAllowed = true
                },
                [ClassCategoryEnum.Lecture] = new ClassFeaturesConfiguration
                {
                    ClassFormConfiguration = new ClassFormConfiguration
                    {
                        IsWwaAllowedValue = false,
                        ShowOnlyDates = false,
                        UseDistanceCoordinator = false,
                        ShowApprovedStaffIds = true,
                        ShowIntegrationCode = true,
                        ShowPayPalKey = true
                    },
                    AreAutomationNotificationsAllowed = true,
                    AreStyleAndBehaviorAllowed = true,
                    ShowClassBehaviorSettings = false,
                    AreInvitationsAllowed = true,
                    AreCertificatesAllowed = false,
                    AreCertificatesAllowedForClassPersons = false,
                    AreReportsAllowed = true,
                    AreFinancialFormsAllowed = false,
                    IsSupervisedByMasterCoordinator = false,
                    IsPropagationToFormerClassesAllowed = false,
                    IsCurrencyAllowed = true,
                    AreMaterialsAllowed = true
                },
                [ClassCategoryEnum.DistanceClass] = new ClassFeaturesConfiguration
                {
                    ClassFormConfiguration = new ClassFormConfiguration
                    {
                        IsWwaAllowedValue = true,
                        ShowOnlyDates = true,
                        UseDistanceCoordinator = true,
                        ShowApprovedStaffIds = false,
                        ShowIntegrationCode = false,
                        ShowPayPalKey = true
                    },
                    AreAutomationNotificationsAllowed = true,
                    AreStyleAndBehaviorAllowed = true,
                    ShowClassBehaviorSettings = false,
                    AreInvitationsAllowed = true,
                    AreCertificatesAllowed = true,
                    AreCertificatesAllowedForClassPersons = false,
                    AreReportsAllowed = false,
                    AreFinancialFormsAllowed = false,
                    IsSupervisedByMasterCoordinator = false,
                    IsPropagationToFormerClassesAllowed = false,
                    IsCurrencyAllowed = false,
                    AreMaterialsAllowed = false
                },
                [ClassCategoryEnum.PrivateMaterialClass] = new ClassFeaturesConfiguration
                {
                    ClassFormConfiguration = new ClassFormConfiguration
                    {
                        IsWwaAllowedValue = false,
                        ShowOnlyDates = true,
                        UseDistanceCoordinator = false,
                        ShowApprovedStaffIds = false,
                        ShowIntegrationCode = false,
                        ShowPayPalKey = false
                    },
                    AreAutomationNotificationsAllowed = false,
                    AreStyleAndBehaviorAllowed = false,
                    ShowClassBehaviorSettings = false,
                    AreInvitationsAllowed = false,
                    AreCertificatesAllowed = false,
                    AreCertificatesAllowedForClassPersons = false,
                    AreReportsAllowed = false,
                    AreFinancialFormsAllowed = false,
                    IsSupervisedByMasterCoordinator = false,
                    IsPropagationToFormerClassesAllowed = false,
                    IsCurrencyAllowed = false,
                    AreMaterialsAllowed = true
                }
            };
        }

        #region common methods

        public ClassDomainInfo GetClassDomainInfo(Class cls)
        {
            var result = new ClassDomainInfo
            {
                IncludesCoordinated = cls.ClassCategoryId == ClassCategoryEnum.Class 
                                      || cls.ClassCategoryId == ClassCategoryEnum.Lecture
                                      || cls.ClassCategoryId == ClassCategoryEnum.PrivateMaterialClass,
                IncludesWwa = cls.IsWwaFormAllowed
            };

            return result;
        }

        public ClassTypeInfo GetClassTypeInfo(ClassTypeEnum classTypeId)
        {
            if (!typeMap.TryGetValue(classTypeId, out var info))
            {
                throw new ArgumentException($"Unknown Class type {classTypeId}.");
            }

            var result = new ClassTypeInfo(info.Id, info.Category, info.Topic, info.Shape);
            return result;
        }

        public HashSet<ClassTypeEnum> GetClassTypesByClassCategoryId(ClassCategoryEnum classCategoryId)
        {
            ClassTypeCategory category;
            switch (classCategoryId)
            {
                case ClassCategoryEnum.Class:
                case ClassCategoryEnum.DistanceClass:
                case ClassCategoryEnum.PrivateMaterialClass:
                    category = ClassTypeCategory.Class;
                    break;
                    
                case ClassCategoryEnum.Lecture:
                    category = ClassTypeCategory.Lecture;
                    break;
               
                default:
                    return new HashSet<ClassTypeEnum>();
            }

            var result = new HashSet<ClassTypeEnum>(types.Where(x => x.Category == category).Select(x => x.Id));
            return result;
        }

        public HashSet<ClassTypeEnum> GetAllowedClassTypesForFormerClasses()
        {
            var result = new HashSet<ClassTypeEnum>(types.Where(x => x.Category == ClassTypeCategory.Class).Select(x => x.Id));
            return result;
        }

        public HashSet<ClassTypeEnum> GetAllowedClassTypesForFormerClassFiltering()
        {
            var result = new HashSet<ClassTypeEnum>(types
                .Where(x => x.Category == ClassTypeCategory.Class && x.Shape == ClassTypeShape.InPerson)
                .Select(x => x.Id));
            return result;
        }

        public HashSet<ClassTypeEnum> GetSynonymousClassTypesForFormerClassFiltering(ClassTypeEnum? classTypeId)
        {
            if (!classTypeId.HasValue)
            {
                return null;
            }

            var info = GetClassTypeInfo(classTypeId.Value);
            if (info.Category != ClassTypeCategory.Class)
            {
                throw new ArgumentException($"Class type {classTypeId} with category {info.Category} is invalid in the formers class context.");
            }

            // gets synonymous class types
            var result = new HashSet<ClassTypeEnum>(types
                .Where(x => x.Category == info.Category && x.Topic == info.Topic)
                .Select(x => x.Id));
            return result;
        }

        public ClassTypeEnum? NormalizeClassTypeSynonymForFormerClassFiltering(ClassTypeEnum? classTypeId)
        {
            if (!classTypeId.HasValue)
                return null;

            // gets info and ignores non-class items
            var info = GetClassTypeInfo(classTypeId.Value);
            if (info.Category != ClassTypeCategory.Class)
                return null;

            var result = types.First(x => x.Category == info.Category && x.Topic == info.Topic && x.Shape == ClassTypeShape.InPerson).Id;
            return result;
        }

        #endregion

        #region business specific methods

        public bool AreMaterialsAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreMaterialsAllowed;
        }

        public bool AreCertificatesAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreCertificatesAllowed;
        }

        public bool AreCertificatesAllowedForClassPersons(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreCertificatesAllowedForClassPersons;
        }

        public bool IsSupervisedByMasterCoordinator(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).IsSupervisedByMasterCoordinator;
        }

        public bool AreFinancialFormsAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreFinancialFormsAllowed;
        }

        public bool AreReportsAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreReportsAllowed;
        }

        public bool ShowClassBehaviorSettings(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).ShowClassBehaviorSettings;
        }

        public bool IsPropagationToFormerClassesAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).IsPropagationToFormerClassesAllowed;
        }

        public bool IsCurrencyAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).IsCurrencyAllowed;
        }

        public bool AreInvitationsAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreInvitationsAllowed;
        }

        public bool AreStyleAndBehaviorAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreStyleAndBehaviorAllowed;
        }

        public bool AreAutomationNotificationsAllowed(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).AreAutomationNotificationsAllowed;
        }

        public ClassFormConfiguration GetClassFormConfigurationByClassCategoryId(ClassCategoryEnum classCategoryId)
        {
            return GetConfigByClassCategoryId(classCategoryId).ClassFormConfiguration;
        }

        #endregion

        #region private methods

        public ClassFeaturesConfiguration GetConfigByClassCategoryId(ClassCategoryEnum classCategoryId)
        {
            if (!classFormConfigurationMap.TryGetValue(classCategoryId, out var result))
            {
                throw new ArgumentException($"Unknown class category {classCategoryId}.");
            }

            return result;
        }

        #endregion
    }
}
