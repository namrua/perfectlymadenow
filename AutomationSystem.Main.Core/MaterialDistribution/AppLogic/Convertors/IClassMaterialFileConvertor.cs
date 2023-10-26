using System.Collections.Generic;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors
{
    /// <summary>
    /// Converts objects related to ClassMaterialFile entity
    /// </summary>
    public interface IClassMaterialFileConvertor
    {

        // initializes ClassMaterialsFileForEdit
        ClassMaterialFileForEdit InitializeClassmaterialFileForEdit();

        // convers ClassMaterialFile to ClassMaterialFileForm
        ClassMaterialFileForm ConvertoToClassMaterialFileForm(ClassMaterialFile materialFile, long classId);

        // converts ClassMaterialFile to ClassMaterialFileDetail
        ClassMaterialFileDetail ConvertToClassMaterialFileDetail(ClassMaterialFile materialFile);

        // converts ClassMaterialFile to ClassMaterialFileDetail with download counts
        ClassMaterialFileDetail ConvertToClassMaterialFileDetailWithDownloadCounts(ClassMaterialFile materialFile, IDictionary<long, int> countsByMaterialFileId);

        // converts ClassMaterialFileForm to ClassMaterialFile
        ClassMaterialFile ConvertToClassMaterialFile(ClassMaterialFileForm form, long classMaterialId, long fileId);

    }
}
