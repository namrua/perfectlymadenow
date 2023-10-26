using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using ClosedXML.Excel;

namespace AutomationSystem.Main.Core.Registrations.System
{
    /// <summary>
    /// Resolves RegistrationType relevant logic and business rules
    /// </summary>
    public class RegistrationTypeResolver : IRegistrationTypeResolver
    {
        private readonly HashSet<RegistrationTypeEnum> studentTypes;
        private readonly HashSet<RegistrationTypeEnum> newTypes;
        private readonly HashSet<RegistrationTypeEnum> childTypes;
        private readonly HashSet<RegistrationTypeEnum> wwaTypes;
        private readonly HashSet<RegistrationTypeEnum> defaultReviewTypes;
        private readonly Dictionary<ClassCategoryEnum, HashSet<RegistrationTypeEnum>> typesByCategory;
        private readonly Dictionary<ClassTypeTopic, HashSet<RegistrationTypeEnum>> reviewTypesByClassTopic;
        private readonly HashSet<ClassTypeTopic> topicsWithoutChildRegistrations;
        private readonly IClassTypeResolver classTypeResolver;

        private readonly IDictionary<RegistrationTypeEnum, RegistrationFormTypeEnum> formTypeMap = new Dictionary<RegistrationTypeEnum, RegistrationFormTypeEnum>();

        // constructor
        public RegistrationTypeResolver(IClassTypeResolver classTypeResolver)
        {
            this.classTypeResolver = classTypeResolver;

            // adult types
            studentTypes = new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.NewAdult,
                RegistrationTypeEnum.NewAdultWeekOfClass,
                RegistrationTypeEnum.ReviewAdult,
                RegistrationTypeEnum.ApprovedGuest,
                RegistrationTypeEnum.LectureRegistration,
                RegistrationTypeEnum.MaterialRegistration
            };
            
            // child types
            childTypes = new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.NewChild,
                RegistrationTypeEnum.ReviewChild
            };

            // new types
            newTypes = new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.NewAdult,
                RegistrationTypeEnum.NewAdultWeekOfClass,
                RegistrationTypeEnum.NewChild
            };

            // WWA types
            wwaTypes = new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.WWA,
            };

            // review types
            defaultReviewTypes = new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.ReviewAdult,
                RegistrationTypeEnum.ReviewChild
            };
            reviewTypesByClassTopic = new Dictionary<ClassTypeTopic, HashSet<RegistrationTypeEnum>>
            {
                {
                    ClassTypeTopic.Basic2,
                    new HashSet<RegistrationTypeEnum>
                    {
                        RegistrationTypeEnum.NewAdult,
                        RegistrationTypeEnum.NewAdultWeekOfClass,
                        RegistrationTypeEnum.ReviewAdult
                    }
                }
            };

            // types by class category
            typesByCategory = new Dictionary<ClassCategoryEnum, HashSet<RegistrationTypeEnum>>
            {
                {
                    ClassCategoryEnum.Class, new HashSet<RegistrationTypeEnum>
                    {
                        RegistrationTypeEnum.NewAdult,
                        RegistrationTypeEnum.NewAdultWeekOfClass,
                        RegistrationTypeEnum.NewChild,
                        RegistrationTypeEnum.ReviewAdult,
                        RegistrationTypeEnum.ReviewChild,
                        RegistrationTypeEnum.WWA
                    }
                },
                {
                    ClassCategoryEnum.Lecture, new HashSet<RegistrationTypeEnum>
                    {
                        RegistrationTypeEnum.LectureRegistration
                    }
                },
                {
                    ClassCategoryEnum.DistanceClass, new HashSet<RegistrationTypeEnum>
                    {
                        RegistrationTypeEnum.WWA
                    }
                },
                {   ClassCategoryEnum.PrivateMaterialClass, new HashSet<RegistrationTypeEnum>
                    {
                        RegistrationTypeEnum.MaterialRegistration
                    }
                }
            };

            // topics without child registrations
            topicsWithoutChildRegistrations = new HashSet<ClassTypeTopic>
            {
                ClassTypeTopic.Business,
                ClassTypeTopic.Basic2
            };

            FillFormMap();
        }

        #region common methods

        public List<RegistrationTypeEnum> GetRegistrationTypeByClassCategoryId(ClassCategoryEnum classCategoryId)
        {
            if (!typesByCategory.TryGetValue(classCategoryId, out var preResult))
            {
                throw new ArgumentException($"Unknown class category {classCategoryId}.");
            }

            return preResult.ToList();
        }

        public List<RegistrationTypeEnum> GetRegistrationTypeByClassCategoryIdAndTopic(ClassCategoryEnum classCategoryId, ClassTypeTopic topic)
        {
            var registrationTypeByCategory = GetRegistrationTypeByClassCategoryId(classCategoryId)
                .Where(x => !topicsWithoutChildRegistrations.Contains(topic) || !childTypes.Contains(x))
                .ToList();
            return registrationTypeByCategory;
        }

        #endregion

        #region business specific methods
        
        public HashSet<RegistrationTypeEnum> GetNewRegistrationTypes()
        {
            return new HashSet<RegistrationTypeEnum>(newTypes);
        }

        public bool IsNewRegistration(RegistrationTypeEnum registrationTypeId)
        {
            var result = newTypes.Contains(registrationTypeId);
            return result;
        }

        public RegistrationFormTypeEnum GetRegistrationFormType(RegistrationTypeEnum registrationTypeId)
        {
            if (!formTypeMap.TryGetValue(registrationTypeId, out var result))
            {
                throw new ArgumentException($"Registration type {registrationTypeId} is not supported.");
            }

            return result;
        }

        public bool IsReviewedRegistration(RegistrationTypeEnum registrationTypeId)
        {
            var result = defaultReviewTypes.Contains(registrationTypeId);
            return result;
        }

        public bool NeedsReview(RegistrationTypeEnum registrationTypeId, ClassTypeEnum classTypeId)
        {
            bool result;
            var classTypeInfo = classTypeResolver.GetClassTypeInfo(classTypeId);
            if (reviewTypesByClassTopic.TryGetValue(classTypeInfo.Topic, out var reviewedTypes))
            {
                result = reviewedTypes.Contains(registrationTypeId);
                return result;
            }

            result = defaultReviewTypes.Contains(registrationTypeId);
            return result;
        }

        public bool IsWwaRegistration(RegistrationTypeEnum registrationTypeId)
        {
            var result = wwaTypes.Contains(registrationTypeId);
            return result;
        }

        public string GetRegistrationTypeCode(RegistrationTypeEnum registrationTypeId, ClassCategoryEnum? classCategoryId = null)
        {
            switch (registrationTypeId)
            {
                case RegistrationTypeEnum.NewAdult:
                case RegistrationTypeEnum.NewAdultWeekOfClass:
                    return "NA";
                case RegistrationTypeEnum.NewChild:
                    return "NC";
                case RegistrationTypeEnum.ReviewAdult:
                    return "RA";
                case RegistrationTypeEnum.ReviewChild:
                    return "RC";
                case RegistrationTypeEnum.WWA:
                    return classCategoryId == ClassCategoryEnum.DistanceClass ? "DWWA-ZL" : "WWA-ZL";
                case RegistrationTypeEnum.ApprovedGuest:
                    return "AG";
                case RegistrationTypeEnum.LectureRegistration:
                    return "L";
                case RegistrationTypeEnum.MaterialRegistration:
                    return "MR";
                default:
                    throw new ArgumentException($"Unsupported registration type {registrationTypeId}.");
            }
        }

        #endregion

        #region private methods

        private void FillFormMap()
        {
            studentTypes.ForEach(x => formTypeMap.Add(x, RegistrationFormTypeEnum.Adult));
            childTypes.ForEach(x => formTypeMap.Add(x, RegistrationFormTypeEnum.Child));
            wwaTypes.ForEach(x => formTypeMap.Add(x, RegistrationFormTypeEnum.WWA));
        }

        #endregion
    }

}
