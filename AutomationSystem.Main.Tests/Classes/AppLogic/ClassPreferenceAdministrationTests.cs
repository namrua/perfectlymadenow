using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Tests.Profiles.TestingHelpers;
using AutomationSystem.Shared.Contract.Files.System;
using Moq;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Tests.Classes.AppLogic
{
    public class ClassPreferenceAdministrationTests
    {
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<ICoreFileService> coreFileServiceMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IClassExpenseFactory> classExpenseFactoryMock;
        private readonly Mock<IClassPreferenceFactory> classPreferenceFactoryMock;
        private readonly Mock<IClassPreferenceDatabaseLayer> classPreferenceDbMock;

        public ClassPreferenceAdministrationTests()
        {
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            coreFileServiceMock = new Mock<ICoreFileService>();
            identityResolverMock = new Mock<IIdentityResolver>();
            mainMapperMock = new Mock<IMainMapper>();
            classExpenseFactoryMock = new Mock<IClassExpenseFactory>();
            classPreferenceFactoryMock = new Mock<IClassPreferenceFactory>();
            classPreferenceDbMock = new Mock<IClassPreferenceDatabaseLayer>();
        }

        #region GetClassPreferenceDetail() tests
        [Fact]
        public void GetClassPreferenceDetail_ExpectedIncludesPassToGetProfileById()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ClassPreferenceDetail>(It.IsAny<ClassPreference>())).Returns(new ClassPreferenceDetail());
            var admin = CreateAdministration();

            // act
            admin.GetClassPreferenceDetail(1);

            // assert
            profileDbMock.Verify(e => e.GetProfileById(
                1,
                ProfileIncludes.ClassPreferenceClassPreferenceExpenses | ProfileIncludes.ClassPreferenceLocationInfo | ProfileIncludes.ClassPreferenceCurrency),
                Times.Once);
        }

        [Fact]
        public void GetClassPreferenceDetail_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = CreateProfile();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfile()
                .Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetClassPreferenceDetail(1));
            identityResolverMock.VerifyCheckEntitleForProfile(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void GetClassPreferenceDetail_ProfileClassPreference_IsMappedToClassPreferenceDetail()
        {
            // arrange
            var profile = CreateProfile();
            var detail = new ClassPreferenceDetail();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<ClassPreferenceDetail>(profile.ClassPreference)).Returns(detail);
            var admin = CreateAdministration();

            // act
            var result = admin.GetClassPreferenceDetail(1);

            // assert
            Assert.Equal(1, result.ProfileId);
            Assert.Same(detail, result);
            mainMapperMock.Verify(e => e.Map<ClassPreferenceDetail>(profile.ClassPreference), Times.Once);
        }
        #endregion

        #region GetClassPreferenceForEditByProfileId() tests
        [Fact]
        public void GetClassPreferenceForEditByProfileId_ExpectedIncludesPassToGetProfileById()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            classPreferenceFactoryMock.Setup(e => e.CreateClassPreferenceForEdit(It.IsAny<long>())).Returns(new ClassPreferenceForEdit());
            mainMapperMock.Setup(e => e.Map<ClassPreferenceForm>(It.IsAny<ClassPreference>())).Returns(new ClassPreferenceForm());
            var admin = CreateAdministration();

            // act
            admin.GetClassPreferenceForEditByProfileId(1);

            // assert
            profileDbMock.Verify(e => e.GetProfileById(1, ProfileIncludes.ClassPreference), Times.Once);
        }

        [Fact]
        public void GetClassPreferenceForEditByProfileId_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = CreateProfile();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfile().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetExpenseLayoutForEditByProfileId(1));
            identityResolverMock.VerifyCheckEntitleForProfile(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void GetClassPreferenceForEditByProfileId_ForProfileId_FactoryCreatesClassPreferenceForEdit()
        {
            // arrange
            var forEdit = new ClassPreferenceForEdit();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            classPreferenceFactoryMock.Setup(e => e.CreateClassPreferenceForEdit(It.IsAny<long>())).Returns(forEdit);
            mainMapperMock.Setup(e => e.Map<ClassPreferenceForm>(It.IsAny<ClassPreference>())).Returns(new ClassPreferenceForm());
            var admin = CreateAdministration();

            // act
            var result = admin.GetClassPreferenceForEditByProfileId(1);

            // assert
            Assert.Same(forEdit, result);
            classPreferenceFactoryMock.Verify(e => e.CreateClassPreferenceForEdit(1), Times.Once);
        }

        [Fact]
        public void GetClassPreferenceForEditByProfileId_ProfileClassPreference_IsMappedToForm()
        {
            // arrange
            var profile = CreateProfile();
            var form = new ClassPreferenceForm();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            classPreferenceFactoryMock.Setup(e => e.CreateClassPreferenceForEdit(It.IsAny<long>())).Returns(new ClassPreferenceForEdit());
            mainMapperMock.Setup(e => e.Map<ClassPreferenceForm>(It.IsAny<ClassPreference>())).Returns(form);
            var admin = CreateAdministration();

            // act
            var result = admin.GetClassPreferenceForEditByProfileId(1);

            // assert
            Assert.Same(form, result.Form);
            Assert.Equal(1, result.Form.ProfileId);
            mainMapperMock.Verify(e => e.Map<ClassPreferenceForm>(profile.ClassPreference), Times.Once);
        }
        #endregion

        #region GetClassPreferenceForEditByForm() tests
        [Fact]
        public void GetClassPreferenceForEditByForm_NoAccessToProfileEntitle_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            identityResolverMock.Setup(e => e.CheckEntitle(It.IsAny<Entitle>())).Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetClassPreferenceForEditByForm(new ClassPreferenceForm()));
            identityResolverMock.Verify(e => e.CheckEntitle(Entitle.MainProfiles), Times.Once);
        }

        [Fact]
        public void GetClassPreferenceForEditByForm_ForClassPreferenceForm_ReturnsClassPreferenceForEdit()
        {
            // arrange
            var form = new ClassPreferenceForm
            {
                ProfileId = 1
            };
            var forEdit = new ClassPreferenceForEdit();
            classPreferenceFactoryMock.Setup(e => e.CreateClassPreferenceForEdit(It.IsAny<long>())).Returns(forEdit);
            var admin = CreateAdministration();

            // act
            var result = admin.GetClassPreferenceForEditByForm(form);

            // assert
            Assert.Same(forEdit, result);
            Assert.Same(form, result.Form);
            classPreferenceFactoryMock.Verify(e => e.CreateClassPreferenceForEdit(1), Times.Once);
        }
        #endregion

        #region SaveClassPreference() tests
        [Fact]
        public void SaveClassPreference_ExpectedIncludesPassToGetProfileById()
        {
            // arrange
            var form = new ClassPreferenceForm
            {
                ProfileId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ClassPreference>(It.IsAny<ClassPreferenceForm>())).Returns(new ClassPreference());
            var admin = CreateAdministration();

            // act
            admin.SaveClassPreference(form, null, null);

            // assert
            profileDbMock.Verify(e => e.GetProfileById(1, ProfileIncludes.ClassPreference), Times.Once);
        }

        [Fact]
        public void SaveClassPreference_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = CreateProfile();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfile().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SaveClassPreference(new ClassPreferenceForm(), null, null));
            identityResolverMock.VerifyCheckEntitleForProfile(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void SaveClassPreference_ClassPreferenceForm_IsMappedAndUpdatedInDb()
        {
            // arrange
            var form = new ClassPreferenceForm
            {
                ProfileId = 1
            };
            var classPreference = new ClassPreference();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Profile());
            mainMapperMock.Setup(e => e.Map<ClassPreference>(It.IsAny<ClassPreferenceForm>())).Returns(classPreference);
            var admin = CreateAdministration();

            // act
            admin.SaveClassPreference(form, null, null);

            // assert
            mainMapperMock.Verify(e => e.Map<ClassPreference>(form), Times.Once);
            classPreferenceDbMock.Verify(e => e.UpdateClassPreference(1, It.Is<ClassPreference>(x => x.Equals(classPreference) && !x.HeaderPictureId.HasValue), false), Times.Once);
            VerifyInsertFileNever();
            coreFileServiceMock.Verify(e => e.DeleteFile(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void SaveClassPreference_PictureOnInput_InsertFileAndUpdateClassPreferenceCalled()
        {
            // arrange
            var stream = new MemoryStream();
            var profile = CreateProfile();
            var form = new ClassPreferenceForm
            {
                ProfileId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<ClassPreference>(It.IsAny<ClassPreferenceForm>())).Returns(new ClassPreference());
            SetupInsertFile(100);
            var admin = CreateAdministration();

            // act
            admin.SaveClassPreference(form, stream, "file");

            // assert
            classPreferenceDbMock.Verify(e => e.UpdateClassPreference(1, It.Is<ClassPreference>(x => x.HeaderPictureId == 100), true), Times.Once);
            coreFileServiceMock.Verify(e => e.DeleteFile(It.IsAny<long>()), Times.Never);
            VerifyInsertFile(stream, 1, "file");
        }

        [Fact]
        public void SaveClassPreference_PictureInDbAndOnInput_UpdateClassPreferenceAndInsertFileAndDeleteOriginFileCalled()
        {
            // arrange
            var stream = new MemoryStream();
            var profile = CreateProfile(50);
            var form = new ClassPreferenceForm
            {
                ProfileId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<ClassPreference>(It.IsAny<ClassPreferenceForm>())).Returns(new ClassPreference());
            SetupInsertFile(100);
            var admin = CreateAdministration();

            // act
            admin.SaveClassPreference(form, stream, "file");

            // assert
            classPreferenceDbMock.Verify(e => e.UpdateClassPreference(1, It.Is<ClassPreference>(x => x.HeaderPictureId == 100), true), Times.Once);
            coreFileServiceMock.Verify(e => e.DeleteFile(50), Times.Once);
            VerifyInsertFile(stream, 1, "file");
        }

        [Fact]
        public void SaveClassPreference_PictureInDbAndRemovePicture_UpdateClassPreferenceAndDeleteFileCalled()
        {
            // arrange
            var profile = CreateProfile(50);
            var form = new ClassPreferenceForm
            {
                ProfileId = 1,
                RemoveHeaderPicture = true
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<ClassPreference>(It.IsAny<ClassPreferenceForm>())).Returns(new ClassPreference());
            var admin = CreateAdministration();

            // act
            admin.SaveClassPreference(form, null, null);

            // assert
            classPreferenceDbMock.Verify(e => e.UpdateClassPreference(1, It.Is<ClassPreference>(x => x.HeaderPictureId == null), true), Times.Once);
            coreFileServiceMock.Verify(e => e.DeleteFile(50), Times.Once);
            VerifyInsertFileNever();
        }

        [Fact]
        public void SaveClassPreference_PictureInDb_UpdateClassPreferenceCalled()
        {
            // arrange
            var profile = CreateProfile(50);
            var form = new ClassPreferenceForm
            {
                ProfileId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            mainMapperMock.Setup(e => e.Map<ClassPreference>(It.IsAny<ClassPreferenceForm>())).Returns(new ClassPreference());
            var admin = CreateAdministration();

            // act
            admin.SaveClassPreference(form, null, null);

            // assert
            classPreferenceDbMock.Verify(e => e.UpdateClassPreference(1, It.IsAny<ClassPreference>(), false), Times.Once);
            coreFileServiceMock.Verify(e => e.DeleteFile(It.IsAny<long>()), Times.Never);
            VerifyInsertFileNever();
        }
        #endregion

        #region GetExpenseLayoutForEditByProfileId() tests
        [Fact]
        public void GetExpenseLayoutForEditByProfileId_ExpectedIncludesPassToGetProfileById()
        {
            // arrange
            var profile = CreateProfile();
            var forEdit = new ExpensesLayoutForEdit
            {
                Form = new ExpensesLayoutForm()
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            classExpenseFactoryMock.Setup(e => e.CreateExpensesLayoutForEdit(It.IsAny<Currency>())).Returns(forEdit);
            mainMapperMock.Setup(e => e.Map<ExpensesLayoutForm>(It.IsAny<List<ClassPreferenceExpense>>())).Returns(new ExpensesLayoutForm());
            var admin = CreateAdministration();

            // act
            admin.GetExpenseLayoutForEditByProfileId(1);

            // assert
            profileDbMock.Verify(e => e.GetProfileById(1, ProfileIncludes.ClassPreferenceClassPreferenceExpenses | ProfileIncludes.ClassPreferenceCurrency), Times.Once);
        }

        [Fact]
        public void GetExpenseLayoutForEditByProfileId_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = CreateProfile();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfile().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetExpenseLayoutForEditByProfileId(1));
            identityResolverMock.VerifyCheckEntitleForProfile(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void GetExpenseLayoutForEditByProfileId_FactoryCreatesExpenseLayoutForEdit()
        {
            // arrange
            var profile = CreateProfile();
            var forEdit = new ExpensesLayoutForEdit
            {
                Form = new ExpensesLayoutForm
                {
                    EntityId = 1
                }
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            classExpenseFactoryMock.Setup(e => e.CreateExpensesLayoutForEdit(It.IsAny<Currency>())).Returns(forEdit);
            mainMapperMock.Setup(e => e.Map<ExpensesLayoutForm>(It.IsAny<List<ClassPreferenceExpense>>())).Returns(new ExpensesLayoutForm());
            var admin = CreateAdministration();

            // act
            var result = admin.GetExpenseLayoutForEditByProfileId(1);

            // assert
            Assert.Same(forEdit, result);
            classExpenseFactoryMock.Verify(e => e.CreateExpensesLayoutForEdit(profile.ClassPreference.Currency), Times.Once);
        }

        [Fact]
        public void GetExpenseLayoutForEditByProfileId_ClassPreferenceExpenses_IsMappedToExpenseLayoutForm()
        {
            // arrange
            var profile = CreateProfile();
            var form = new ExpensesLayoutForm();
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            classExpenseFactoryMock.Setup(e => e.CreateExpensesLayoutForEdit(It.IsAny<Currency>())).Returns(new ExpensesLayoutForEdit());
            mainMapperMock.Setup(e => e.Map<ExpensesLayoutForm>(It.IsAny<List<ClassPreferenceExpense>>())).Returns(form);
            var admin = CreateAdministration();

            // act
            var result = admin.GetExpenseLayoutForEditByProfileId(1);

            // assert
            Assert.Equal(profile.ProfileId, result.Form.EntityId);
            Assert.Same(form, result.Form);
            mainMapperMock.Verify(e => e.Map<ExpensesLayoutForm>(profile.ClassPreference.ClassPreferenceExpenses), Times.Once);
        }
        #endregion

        #region GetExpenseLayoutForEditByForm() tests
        [Fact]
        public void GetExpenseLayoutForEditByForm_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var profile = CreateProfile();
            var form = new ExpensesLayoutForm
            {
                EntityId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.GetExpenseLayoutForEditByForm(form));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void GetExpenseLayoutForEditByForm_IncludesPassToGetProfileById()
        {
            // arrange
            var profile = CreateProfile();
            var form = new ExpensesLayoutForm
            {
                EntityId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            classExpenseFactoryMock.Setup(e => e.CreateExpensesLayoutForEdit(It.IsAny<Currency>())).Returns(new ExpensesLayoutForEdit());
            var admin = CreateAdministration();

            // act
            admin.GetExpenseLayoutForEditByForm(form);

            // assert
            profileDbMock.Verify(e => e.GetProfileById(1, ProfileIncludes.ClassPreferenceCurrency), Times.Once);
        }

        [Fact]
        public void GetExpenseLayoutForEditByForm_FactoryCreatesExpenseLayoutForEdit()
        {
            // arrange
            var profile = CreateProfile();
            var forEdit = new ExpensesLayoutForEdit
            {
                Form = new ExpensesLayoutForm
                {
                    EntityId = 1
                }
            };
            var form = new ExpensesLayoutForm
            {
                EntityId = 1
            };
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(profile);
            classExpenseFactoryMock.Setup(e => e.CreateExpensesLayoutForEdit(It.IsAny<Currency>())).Returns(forEdit);
            var admin = CreateAdministration();

            // act
            var result = admin.GetExpenseLayoutForEditByForm(form);

            // assert
            Assert.Same(forEdit, result);
            Assert.Same(form, result.Form);
            classExpenseFactoryMock.Verify(e => e.CreateExpensesLayoutForEdit(profile.ClassPreference.Currency), Times.Once);
        }
        #endregion

        #region SaveExpenseLayout() tests
        [Fact]
        public void SaveExpenseLayout_NoAccessToProfile_ThrowsEntitleAccessDeniedException()
        {
            // arrange
            var form = new ExpensesLayoutForm
            {
                EntityId = 1
            };
            identityResolverMock.SetupCheckEntitleForProfileId().Throws(new EntitleAccessDeniedException("", Entitle.MainProfiles));
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<EntitleAccessDeniedException>(() => admin.SaveExpenseLayout(form));
            identityResolverMock.VerifyCheckEntitleForProfileId(Entitle.MainProfiles, 1, Times.Once());
        }

        [Fact]
        public void SaveExpenseLayout_ClassPreferenceForm_IsMappedToClassPreferenceExpensesAndUpdated()
        {
            // arrange
            var form = new ExpensesLayoutForm
            {
                EntityId = 1
            };
            var expenses = new List<ClassPreferenceExpense>();
            mainMapperMock.Setup(e => e.Map<List<ClassPreferenceExpense>>(It.IsAny<ExpensesLayoutForm>())).Returns(expenses);
            var admin = CreateAdministration();

            // act
            admin.SaveExpenseLayout(form);

            // assert
            mainMapperMock.Verify(e => e.Map<List<ClassPreferenceExpense>>(form), Times.Once);
            classPreferenceDbMock.Verify(e => e.UpdateClassPreferenceExpenses(1, expenses), Times.Once);
        }
        #endregion

        #region private methods
        private ClassPreferenceAdministration CreateAdministration()
        {
            return new ClassPreferenceAdministration(
                profileDbMock.Object,
                coreFileServiceMock.Object,
                identityResolverMock.Object,
                mainMapperMock.Object,
                classExpenseFactoryMock.Object,
                classPreferenceFactoryMock.Object,
                classPreferenceDbMock.Object);
        }

        private Profile CreateProfile(long? headerPictureId = null)
        {
            return new Profile
            {
                ProfileId = 1,
                ClassPreference = new ClassPreference
                {
                    HeaderPictureId = headerPictureId,
                    ClassPreferenceExpenses = new List<ClassPreferenceExpense>()
                }
            };
        }

        private void SetupInsertFile(long fileId)
        {
            coreFileServiceMock.Setup(
                   e => e.InsertFile(
                       It.IsAny<Stream>(),
                       It.IsAny<string>(),
                       It.IsAny<string>(),
                       It.IsAny<FileTypeEnum>(),
                       It.IsAny<LanguageEnum?>(),
                       It.IsAny<bool>()))
                .Returns(fileId);
        }

        private void VerifyInsertFile(Stream content, long profileId, string fileName)
        {
            var name = string.Format(ClassPreferenceAdministration.HeaderPictureNamePattern, profileId);
            coreFileServiceMock.Verify(e => e.InsertFile(content, name, fileName, FileTypeEnum.Jpg, null, true), Times.Once);
        }

        private void VerifyInsertFileNever()
        {
            coreFileServiceMock.Verify(
                e => e.InsertFile(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<FileTypeEnum>(),
                    It.IsAny<LanguageEnum?>(),
                    It.IsAny<bool>()),
                Times.Never);
        }
        #endregion
    }
}
