using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.Utilities.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{
    /// <summary>
    /// Converts objects related to ClassMaterialRecipient entity
    /// </summary>
    public class ClassMaterialRecipientConvertor : IClassMaterialRecipientConvertor
    {
        private readonly IMaterialPasswordGenerator materialPasswordGenerator;

        private readonly Lazy<Dictionary<LanguageEnum, IEnumItem>> languageMap;

        // constructor
        public ClassMaterialRecipientConvertor(IEnumDatabaseLayer enumDb, IMaterialPasswordGenerator materialPasswordGenerator)
        {
            this.materialPasswordGenerator = materialPasswordGenerator;
            languageMap = new Lazy<Dictionary<LanguageEnum, IEnumItem>>(() =>
                enumDb.GetItemsByFilter(EnumTypeEnum.Language).ToDictionary(x => (LanguageEnum)x.Id));
        }

        // creates initial ClassMaterialRecipient object
        public ClassMaterialRecipient CreateInitialClassMaterialRecipient(long classMaterialId, RecipientId recipientId, LanguageEnum? preselectedLanguageId = null)
        {
            var result = new ClassMaterialRecipient
            {
                ClassMaterialId = classMaterialId,
                RecipientTypeId = recipientId.TypeId,
                RecipientId = recipientId.Id,
                Password = materialPasswordGenerator.GeneratePassword(),
                RequestCode = RandomStringGenerator.GenerateRequestCode(),
                DownloadLimit = 1,
                LanguageId = preselectedLanguageId
            };
            return result;
        }

        // initializes MaterialRecipientForEdit
        public MaterialRecipientForEdit InitializeMaterialRecipientForEdit(Class cls)
        {
            var result = new MaterialRecipientForEdit();
            result.Languages.Add(languageMap.Value[cls.OriginLanguageId]);
            if (cls.TransLanguageId.HasValue)
                result.Languages.Add(languageMap.Value[cls.TransLanguageId.Value]);
            return result;
        }

        // converts ClassMaterialRecipient to MaterialRecipientDetail
        public MaterialRecipientDetail ConvertToMaterialRecipientDetail(ClassMaterialRecipient materialRecipient)
        {
            var result = new MaterialRecipientDetail
            {
                ClassMaterialRecipientId = materialRecipient.ClassMaterialRecipientId,
                RecipientId = new RecipientId(materialRecipient.RecipientTypeId, materialRecipient.RecipientId),
                Password = materialRecipient.Password,
                RequestCode = materialRecipient.RequestCode,
                LanguageId = materialRecipient.LanguageId,
                Language = materialRecipient.LanguageId.HasValue ? languageMap.Value[materialRecipient.LanguageId.Value].Description : null,
                DownloadLimit = materialRecipient.DownloadLimit,
                IsLocked = materialRecipient.IsLocked,
                Locked = materialRecipient.Locked,
                Notified = materialRecipient.Notified
            };
            return result;
        }

        // converts ClassMaterialRecipient to MaterialRecipientForm
        public MaterialRecipientForm ConvertToMaterialRecipientForm(ClassMaterialRecipient materialRecipient)
        {
            var result = new MaterialRecipientForm
            {
                ClassMaterialRecipientId = materialRecipient.ClassMaterialRecipientId,
                RecipientTypeId = materialRecipient.RecipientTypeId,
                RecipientId = materialRecipient.RecipientId,
                Password = materialRecipient.Password,
                LanguageId = materialRecipient.LanguageId,
                DownloadLimit = materialRecipient.DownloadLimit
            };
            return result;
        }

        // converts MaterialRecipientForm to ClassMaterialRecipient
        public ClassMaterialRecipient ConvertToClassMaterialRecipient(MaterialRecipientForm form)
        {
            var result = new ClassMaterialRecipient
            {
                ClassMaterialRecipientId = form.ClassMaterialRecipientId,
                Password = form.Password,
                LanguageId = form.LanguageId,
                DownloadLimit = form.DownloadLimit
            };
            return result;
        }
    }
}
