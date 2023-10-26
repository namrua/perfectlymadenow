using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Files.System;
using System;
using System.IO;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassStyleAdministration : IClassStyleAdministration
    {
        private const string HeaderPictureNamePattern = "Header picture ({0})";

        private readonly IClassDatabaseLayer classDb;
        private readonly ICoreFileService coreFileService;
        private readonly IIdentityResolver identityResolver;
        private readonly IClassTypeResolver classTypeResolver;
        private readonly IClassStyleConvertor classStyleConvertor;

        public ClassStyleAdministration(
            IClassDatabaseLayer classDb,
            ICoreFileService coreFileService,
            IIdentityResolver identityResolver, 
            IClassStyleConvertor classStyleConvertor,
            IClassTypeResolver classTypeResolver)
        {
            this.classDb = classDb;
            this.coreFileService = coreFileService;
            this.identityResolver = identityResolver;
            this.classStyleConvertor = classStyleConvertor;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassStylePageModel GetClassStylePageModel(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassType | ClassIncludes.ClassStyle);
            identityResolver.CheckEntitleForClass(cls);

            var result = new ClassStylePageModel
            {
                Class = ClassConvertor.ConvertToClassShortDetial(cls)
            };

            if (!classTypeResolver.AreStyleAndBehaviorAllowed(cls.ClassCategoryId))
            {
                result.AreStylesAndBehaviorDisabled = true;
                result.StylesAndBehaviorDisabledMessage = "Style & behavior are not available for the class.";
                return result;
            }

            result.Style = classStyleConvertor.ConvertToClassStyleDetail(cls.ClassStyle, classTypeResolver.ShowClassBehaviorSettings(cls.ClassCategoryId));
            return result;
        }

        public ClassStyleForEdit GetClassStyleForEditByClassId(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassStyle);
            identityResolver.CheckEntitleForClass(cls);

            var result = classStyleConvertor.InitializeClassStyleForEdit(classTypeResolver.ShowClassBehaviorSettings(cls.ClassCategoryId));
            result.Form = classStyleConvertor.ConvertToClassStyleForm(cls.ClassStyle, classId);
            return result;
        }

        public ClassStyleForEdit GetClassStyleForEditByForm(ClassStyleForm form)
        {
            var cls = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(cls);

            var result = classStyleConvertor.InitializeClassStyleForEdit(classTypeResolver.ShowClassBehaviorSettings(cls.ClassCategoryId));
            result.Form = form;
            return result;
        }

        public void SaveClassStyle(ClassStyleForm form, Stream headerPictureContent, string headerPictureName)
        {
            var cls = GetClassById(form.ClassId, ClassIncludes.ClassStyle);
            identityResolver.CheckEntitleForClass(cls);

            var isPictureUploaded = headerPictureContent != null;
            var updateHeaderPictureId = false;
            long? newHeaderPictureId = null;
            long? headerPictureIdToDelete = null;

            // when origin header picture should be deleted or replaced,
            // origin header picture id is loaded to be deleted
            if (form.RemoveHeaderPicture || isPictureUploaded)
            {
                // loads headerPictureId to delete
                headerPictureIdToDelete = cls.ClassStyle.HeaderPictureId;
            }

            // handles removing of picture
            if (form.RemoveHeaderPicture)
            {
                updateHeaderPictureId = true;
            }

            // handles new loaded picture
            if (isPictureUploaded)
            {
                newHeaderPictureId = coreFileService.InsertFile(headerPictureContent, 
                    string.Format(HeaderPictureNamePattern, form.ClassId),
                    headerPictureName, FileTypeEnum.Jpg, isPublic: true);
                updateHeaderPictureId = true;
            }

            // saves class style
            var toUpdate = classStyleConvertor.ConverToClassStyle(form, newHeaderPictureId);
            classDb.UpdateClassStyle(form.ClassId, toUpdate, updateHeaderPictureId);

            // deletes origin picture
            if (headerPictureIdToDelete.HasValue)
            {
                coreFileService.DeleteFile(headerPictureIdToDelete.Value);
            }
        }

        #region private methods

        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            return result;
        }

        #endregion
    }
}