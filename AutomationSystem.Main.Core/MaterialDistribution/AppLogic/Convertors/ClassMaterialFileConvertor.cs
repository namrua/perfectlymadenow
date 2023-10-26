using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{

    /// <summary>
    /// Converts objects related to ClassMaterialFile entity
    /// </summary>
    public class ClassMaterialFileConvertor : IClassMaterialFileConvertor
    {

        private readonly IEnumDatabaseLayer enumDb;

        private readonly Lazy<Dictionary<LanguageEnum, IEnumItem>> languageMap;

        // constructor
        public ClassMaterialFileConvertor(IEnumDatabaseLayer enumDb)
        {
            this.enumDb = enumDb;

            languageMap = new Lazy<Dictionary<LanguageEnum, IEnumItem>>(() => 
                enumDb.GetItemsByFilter(EnumTypeEnum.Language).ToDictionary(x => (LanguageEnum)x.Id));
        }


        // initializes ClassMaterialsFileForEdit
        public ClassMaterialFileForEdit InitializeClassmaterialFileForEdit()
        {
            var result = new ClassMaterialFileForEdit
            {
                Languages = languageMap.Value.Values.ToList()
            };
            return result;
        }


        // Converts ClassMaterialFile to ClassMaterialFileDetail
        public ClassMaterialFileDetail ConvertToClassMaterialFileDetail(ClassMaterialFile materialFile)
        {
            var result = new ClassMaterialFileDetail
            {
                ClassMaterialFileId = materialFile.ClassMaterialFileId,
                DisplayName = materialFile.DisplayName,
                LanguageId = materialFile.LanguageId,
                Language = languageMap.Value[materialFile.LanguageId].Description,
                FileId = materialFile.FileId
            };
            return result;
        }


        // converts ClassMaterialFile to ClassMaterialFileDetail with download counts
        public ClassMaterialFileDetail ConvertToClassMaterialFileDetailWithDownloadCounts(
            ClassMaterialFile materialFile, IDictionary<long, int> countsByMaterialFileId)
        {
            var result = ConvertToClassMaterialFileDetail(materialFile);

            // fills up downloads
            if (!countsByMaterialFileId.TryGetValue(materialFile.ClassMaterialFileId, out var downloadCount))
                downloadCount = 0;
            result.DownloadsCount = downloadCount;
            return result;
        }


        // convers ClassMaterialFile to ClassMaterialFileForm
        public ClassMaterialFileForm ConvertoToClassMaterialFileForm(ClassMaterialFile materialFile, long classId)
        {
            var result = new ClassMaterialFileForm
            {
                ClassId = classId,
                ClassMaterialFileId = materialFile.ClassMaterialFileId,
                DisplayName = materialFile.DisplayName,
                LanguageId = materialFile.LanguageId
            };
            return result;
        }


        // converts ClassMaterialFileForm to ClassMaterialFile
        public ClassMaterialFile ConvertToClassMaterialFile(ClassMaterialFileForm form, long classMaterialId, long fileId)
        {
            var result = new ClassMaterialFile
            {
                ClassMaterialFileId = form.ClassMaterialFileId,
                ClassMaterialId = classMaterialId,
                DisplayName = form.DisplayName,
                LanguageId = form.LanguageId ?? 0,
                FileId = fileId
            };
            return result;
        }


    }

}
