using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic.Models;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.Models.Events;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Model;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceProfiles.AppLogic
{
    public class DistanceProfileAdministrationTests
    {
        private readonly Mock<IPaymentDatabaseLayer> paymentDbMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IDistanceProfileDatabaseLayer> distanceProfileDbMock;
        private readonly Mock<IProfileDatabaseLayer> profileDbMock;
        private readonly Mock<IDistanceProfileFactory> distanceProfileFactoryMock;
        private readonly Mock<IEventDispatcher> eventDispatcherMock;

        public DistanceProfileAdministrationTests()
        {
            paymentDbMock = new Mock<IPaymentDatabaseLayer>();
            mainMapperMock = new Mock<IMainMapper>();
            distanceProfileDbMock = new Mock<IDistanceProfileDatabaseLayer>();
            profileDbMock = new Mock<IProfileDatabaseLayer>();
            distanceProfileFactoryMock = new Mock<IDistanceProfileFactory>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
        }

        #region GetDistanceProfilePageModel() tests
        [Fact]
        public void GetDistanceProfilePageModel_ExpectedIncludesPassToGetDistanceProfiles()
        {
            // arrange
            paymentDbMock.Setup(e => e.GetPayPalKeysByIds(It.IsAny<List<long>>())).Returns(new List<PayPalKey>());
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), It.IsAny<DistanceProfileIncludes>())).Returns(new List<DistanceProfile>());
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>())).Returns(new List<Model.Profile>());
            var admin = CreateAdministration();

            // act
            admin.GetDistanceProfilePageModel();

            // assert
            distanceProfileDbMock.Verify(e => e.GetDistanceProfilesByFilter(
                null,
                DistanceProfileIncludes.Profile
                | DistanceProfileIncludes.PriceList
                | DistanceProfileIncludes.DistanceCoordinatorAddress), Times.Once);
        }

        [Fact]
        public void GetDistanceProfilePageModel_ForFilteredProfiles_ProfilesSetToPageModel()
        {
            // arrange
            var payPalKeys = CreatePayPalKeys();
            var distanceProfiles = CreateDistanceProfiles();
            var profiles = CreateProfiles();
            ProfileFilter filter = null;
            paymentDbMock.Setup(e => e.GetPayPalKeysByIds(It.IsAny<List<long>>())).Returns(payPalKeys);
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfiles);
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>()))
                .Callback(new Action<ProfileFilter, ProfileIncludes>((f, i) => filter = f))
                .Returns(profiles);
            mainMapperMock.Setup(e => e.Map<DistanceProfileListItem>(It.IsAny<DistanceProfile>())).Returns(new DistanceProfileListItem());
            var admin = CreateAdministration();

            // act
            var result = admin.GetDistanceProfilePageModel();

            // assert
            Assert.Collection(filter.ExcludeProfileIds,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item));
            Assert.Collection(result.Profiles,
                item =>
                {
                    Assert.Equal("3", item.Id);
                    Assert.Equal("third", item.Text);
                },
                item =>
                {
                    Assert.Equal("4", item.Id);
                    Assert.Equal("fourth", item.Text);
                });
            profileDbMock.Verify(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>()), Times.Once);
        }

        [Fact]
        public void GetDistanceProfilePageModel_PayPalKeyIsMissing_ThrowsArgumentException()
        {
            // arrange
            var distanceProfiles = CreateDistanceProfiles();
            paymentDbMock.Setup(e => e.GetPayPalKeysByIds(It.IsAny<List<long>>())).Returns(new List<PayPalKey>());
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfiles);
            mainMapperMock.Setup(e => e.Map<DistanceProfileListItem>(It.IsAny<DistanceProfile>())).Returns(new DistanceProfileListItem());
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>())).Returns(new List<Model.Profile>());
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetDistanceProfilePageModel());
        }

        [Fact]
        public void GetDistanceProfilePageModel_DistanceProfileIsMappedToDistanceProfileListItem()
        {
            // arrange
            var distanceProfiles = CreateDistanceProfiles();
            var payPalKeys = CreatePayPalKeys();
            var listItemOne = new DistanceProfileListItem
            {
                DistanceProfileId = 1
            };
            var listItemTwo = new DistanceProfileListItem
            {
                DistanceProfileId = 2
            };
            paymentDbMock.Setup(e => e.GetPayPalKeysByIds(It.IsAny<List<long>>())).Returns(payPalKeys);
            distanceProfileDbMock.Setup(e => e.GetDistanceProfilesByFilter(It.IsAny<DistanceProfileFilter>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfiles);
            mainMapperMock.SetupSequence(e => e.Map<DistanceProfileListItem>(It.IsAny<DistanceProfile>()))
                .Returns(listItemOne)
                .Returns(listItemTwo);
            profileDbMock.Setup(e => e.GetProfilesByFilter(It.IsAny<ProfileFilter>(), It.IsAny<ProfileIncludes>())).Returns(new List<Model.Profile>());
            var admin = CreateAdministration();

            // act
            var result = admin.GetDistanceProfilePageModel();

            // assert
            Assert.Collection(result.Items,
                item =>
                {
                    Assert.Same(listItemOne, item);
                    Assert.Equal("firstPayPalKey", item.PayPalKey);
                },
                item =>
                {
                    Assert.Same(listItemTwo, item);
                    Assert.Equal("secondPayPalKey", item.PayPalKey);
                });
        }
        #endregion

        #region GetDistanceProfileDetailById() tests
        [Fact]
        public void GetDistanceProfileDetailById_ExpectedIncludesPassToGetDistanceProfileById()
        {
            // arrange
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(new PayPalKey());
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(new DistanceProfile());
            mainMapperMock.Setup(e => e.Map<DistanceProfileDetail>(It.IsAny<DistanceProfile>())).Returns(new DistanceProfileDetail());

            var admin = CreateAdministration();

            // act
            admin.GetDistanceProfileDetailById(1);

            // assert
            distanceProfileDbMock.Verify(e => e.GetDistanceProfileById(
                1,
                DistanceProfileIncludes.Profile
                | DistanceProfileIncludes.PriceList
                | DistanceProfileIncludes.DistanceCoordinatorAddress), Times.Once);
        }

        [Fact]
        public void GetDistanceProfileDetailById_PayPalKeyIsMissing_ThrowsArgumentException()
        {
            // arrange
            var distanceProfile = CreateDistanceProfile();
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(default(PayPalKey));
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfile);
            mainMapperMock.Setup(e => e.Map<DistanceProfileDetail>(It.IsAny<DistanceProfile>())).Returns(new DistanceProfileDetail());
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetDistanceProfileDetailById(1));
        }

        [Fact]
        public void GetDistanceProfileDetailById_DistanceProfileIsNull_ThrowsArgumentException()
        {
            // arrange
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(new PayPalKey());
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns((DistanceProfile)null);
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetDistanceProfileDetailById(1));
        }

        [Fact]
        public void GetDistanceProfileDetailById_DistanceProfileIsMappedToDistanceProfileDetail()
        {
            // arrange
            var distanceProfile = CreateDistanceProfile();
            var payPalKey = new PayPalKey
            {
                Name = "PayPalKey"
            };
            var detail = new DistanceProfileDetail
            {
                DistanceProfileId = 1
            };
            paymentDbMock.Setup(e => e.GetPayPalKeyById(It.IsAny<long>())).Returns(payPalKey);
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfile);
            mainMapperMock.Setup(e => e.Map<DistanceProfileDetail>(It.IsAny<DistanceProfile>())).Returns(detail);
            var admin = CreateAdministration();

            // act
            var result = admin.GetDistanceProfileDetailById(1);

            // assert
            Assert.Same(detail, result);
            Assert.Equal("PayPalKey", result.PayPalKey);
        }
        #endregion

        #region GetNewDistanceProfileForEdit() tests
        [Fact]
        public void GetNewDistanceProfileForEdit_ProfileIsNull_ThrowsArgumentException()
        {
            // arrange
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns((Model.Profile)null);
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetDistanceProfileDetailById(1));
        }

        [Fact]
        public void GetNewDistanceProfileForEdit_ForProfileId_ReturnsDistanceProfileForEdit()
        {
            // arrange
            var forEdit = new DistanceProfileForEdit();
            var personHelperMock = new Mock<IPersonHelper>();
            forEdit.Persons = personHelperMock.Object;
            profileDbMock.Setup(e => e.GetProfileById(It.IsAny<long>(), It.IsAny<ProfileIncludes>())).Returns(new Model.Profile());
            distanceProfileFactoryMock.Setup(e => e.CreateDistanceProfileForEdit(It.IsAny<long>(), It.IsAny<long?>(), It.IsAny<long?>())).Returns(forEdit);
            personHelperMock.Setup(e => e.GetDefaultPersonId(It.IsAny<PersonRoleTypeEnum>())).Returns(2);
            var admin = CreateAdministration();

            // act
            var result = admin.GetNewDistanceProfileForEdit(1);

            // assert
            Assert.Equal(2, result.Form.DistanceCoordinatorId);
            Assert.Same(forEdit, result);
            distanceProfileFactoryMock.Verify(e => e.CreateDistanceProfileForEdit(1, null, null), Times.Once);
            personHelperMock.Verify(e => e.GetDefaultPersonId(PersonRoleTypeEnum.DistanceDoordinator), Times.Once);
        }
        #endregion

        #region GetDistanceProfileForEditById() tests
        [Fact]
        public void GetDistanceProfileForEditById_DistanceProfileIsNull_ThrowsArgumentException()
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns((DistanceProfile)null);
            var admin = CreateAdministration();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetDistanceProfileDetailById(1));
        }

        [Fact]
        public void GetDistanceProfileForEditById_ForDistanceProfileId_FactoryCreatesDistanceProfileForEdit()
        {
            // arrange
            var forEdit = new DistanceProfileForEdit();
            var distanceProfile = CreateDistanceProfile();
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfile);
            distanceProfileFactoryMock.Setup(e => e.CreateDistanceProfileForEdit(It.IsAny<long>(), It.IsAny<long?>(), It.IsAny<long?>())).Returns(forEdit);
            mainMapperMock.Setup(e => e.Map<DistanceProfileForm>(It.IsAny<DistanceProfile>())).Returns(new DistanceProfileForm());
            var admin = CreateAdministration();

            // act
            var result = admin.GetDistanceProfileForEditById(2);

            // assert
            Assert.Same(forEdit, result);
            distanceProfileFactoryMock.Verify(e => e.CreateDistanceProfileForEdit(1, 2, 3));
        }

        [Fact]
        public void GetDistanceProfileForEditById_DistanceProfile_IsMappedToDistanceProfileForm()
        {
            // arrange
            var distanceProfile = CreateDistanceProfile();
            var form = new DistanceProfileForm();
            distanceProfileDbMock.Setup(e => e.GetDistanceProfileById(It.IsAny<long>(), It.IsAny<DistanceProfileIncludes>())).Returns(distanceProfile);
            distanceProfileFactoryMock.Setup(e => e.CreateDistanceProfileForEdit(It.IsAny<long>(), It.IsAny<long?>(), It.IsAny<long?>())).Returns(new DistanceProfileForEdit());
            mainMapperMock.Setup(e => e.Map<DistanceProfileForm>(It.IsAny<DistanceProfile>())).Returns(form);
            var admin = CreateAdministration();

            // act
            var result = admin.GetDistanceProfileForEditById(2);

            // assert
            Assert.Same(form, result.Form);
            distanceProfileDbMock.Verify(e => e.GetDistanceProfileById(2, DistanceProfileIncludes.None), Times.Once);
            mainMapperMock.Verify(e => e.Map<DistanceProfileForm>(distanceProfile), Times.Once);
        }
        #endregion

        #region GetDistanceProfileForEditByForm() tests
        [Fact]
        public void GetDistanceProfileForEditByForm_ForForm_ReturnsDistanceProfileForEdit()
        {
            // arrange
            var form = new DistanceProfileForm
            {
                ProfileId = 1,
                CurrentPriceListId = 2,
                CurrentPayPalKeyId = 3
            };
            var forEdit = new DistanceProfileForEdit();
            distanceProfileFactoryMock.Setup(e => e.CreateDistanceProfileForEdit(It.IsAny<long>(), It.IsAny<long?>(), It.IsAny<long?>())).Returns(forEdit);
            var admin = CreateAdministration();

            // act
            var result = admin.GetDistanceProfileForEditByForm(form);

            // assert
            Assert.Same(form, result.Form);
            distanceProfileFactoryMock.Verify(e => e.CreateDistanceProfileForEdit(1, 2, 3), Times.Once);
        }
        #endregion

        #region ActivateDistanceProfile() tests
        [Fact]
        public void ActivateDistanceProfile_ForDistanceProfileId_DistanceProfileIsActivatedAndEventIsRaised()
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.SetDistanceProfileAsActive(It.IsAny<long>()));
            eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<DistanceProfileStatusChangedEvent>()));
            var admin = CreateAdministration();

            // act
            admin.ActivateDistanceProfile(1);

            // assert
            distanceProfileDbMock.Verify(e => e.SetDistanceProfileAsActive(1), Times.Once);
            eventDispatcherMock.Verify(e => e.Dispatch(It.Is<DistanceProfileStatusChangedEvent>(x => x.DistanceProfileId == 1 && x.IsActive)), Times.Once);
        }
        #endregion

        #region DeactivateDistanceProfile() tests
        [Fact]
        public void DeactivateDistanceProfile_ForDistaneProfileId_DistanceProfileIsDeactivatedAndEventIsRaised()
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.SetDistanceProfileAsDeactive(It.IsAny<long>()));
            eventDispatcherMock.Setup(e => e.Dispatch(It.IsAny<DistanceProfileStatusChangedEvent>()));
            var admin = CreateAdministration();

            // act
            admin.DeactivateDistanceProfile(10);

            // assert
            distanceProfileDbMock.Verify(e => e.SetDistanceProfileAsDeactive(10), Times.Once);
            eventDispatcherMock.Verify(e => e.Dispatch(It.Is<DistanceProfileStatusChangedEvent>(x => x.DistanceProfileId == 10 && x.IsActive == false)), Times.Once);
        }
        #endregion

        #region SaveDistanceProfile() tests
        [Fact]
        public void SaveDistanceProfile_DistanceProfileForm_IsMappedToDistanceProfile()
        {
            // arrange
            var form = new DistanceProfileForm
            {
                DistanceProfileId = 1
            };
            mainMapperMock.Setup(e => e.Map<DistanceProfile>(It.IsAny<DistanceProfileForm>())).Returns(new DistanceProfile());
            var admin = CreateAdministration();

            // act
            var result = admin.SaveDistanceProfile(form);

            // assert
            Assert.Equal(1, result);
            mainMapperMock.Verify(e => e.Map<DistanceProfile>(form), Times.Once);
        }

        [Fact]
        public void SaveDistanceProfile_NewDistanceProfile_DistanceProfileInserted()
        {
            // arrange
            var distanceProfile = CreateDistanceProfile();
            mainMapperMock.Setup(e => e.Map<DistanceProfile>(It.IsAny<DistanceProfileForm>())).Returns(distanceProfile);
            distanceProfileDbMock.Setup(e => e.InsertDistanceProfile(It.IsAny<DistanceProfile>())).Returns(10);
            var admin = CreateAdministration();

            // act
            var result = admin.SaveDistanceProfile(new DistanceProfileForm());

            // assert
            Assert.Equal(10, result);
            distanceProfileDbMock.Verify(e => e.InsertDistanceProfile(distanceProfile), Times.Once);
        }

        [Fact]
        public void SaveDistanceProfile__ExistingDistanceProfile_DistanceProfileUpdated()
        {
            // arrange
            var form = new DistanceProfileForm
            {
                DistanceProfileId = 11
            };
            var distanceProfile = CreateDistanceProfile();
            mainMapperMock.Setup(e => e.Map<DistanceProfile>(It.IsAny<DistanceProfileForm>())).Returns(distanceProfile);
            var admin = CreateAdministration();

            // act
            var result = admin.SaveDistanceProfile(form);

            // assert
            Assert.Equal(11, result);
            distanceProfileDbMock.Verify(e => e.UpdateDistanceProfile(distanceProfile), Times.Once);
        }
        #endregion

        #region DeleteDistanceProfile() test
        [Fact]
        public void DeleteDistanceProfile_ProfileIsDeleted()
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.DeleteDistanceProfile(It.IsAny<long>()));
            var admin = CreateAdministration();

            // act
            admin.DeleteDistanceProfile(1);

            // assert
            distanceProfileDbMock.Verify(e => e.DeleteDistanceProfile(1), Times.Once);
        }
        #endregion

        #region private methods
        private List<Model.Profile> CreateProfiles()
        {
            return new List<Model.Profile>
            {
                new Model.Profile
                {
                    ProfileId = 3,
                    Name = "third"
                },
                new Model.Profile
                {
                    ProfileId = 4,
                    Name = "fourth"
                }
            };
        }
        
        private DistanceProfile CreateDistanceProfile()
        {
            return new DistanceProfile
            {
                ProfileId = 1,
                PriceListId = 2,
                PayPalKeyId = 3
            };
        }

        private List<DistanceProfile> CreateDistanceProfiles()
        {
            return new List<DistanceProfile>
            {
                new DistanceProfile
                {
                    ProfileId = 1,
                    PayPalKeyId = 1
                },
                new DistanceProfile
                {
                    ProfileId = 2,
                    PayPalKeyId = 2
                }
            };
        }

        private List<PayPalKey> CreatePayPalKeys()
        {
            return new List<PayPalKey>
            {
                new PayPalKey
                {
                    PayPalKeyId = 1,
                    Name = "firstPayPalKey"
                },
                new PayPalKey
                {
                    PayPalKeyId = 2,
                    Name = "secondPayPalKey"
                }
            };
        }

        private DistanceProfileAdministration CreateAdministration()
        {
            return new DistanceProfileAdministration(
                paymentDbMock.Object,
                mainMapperMock.Object,
                distanceProfileDbMock.Object,
                profileDbMock.Object,
                distanceProfileFactoryMock.Object,
                eventDispatcherMock.Object);
        }
        #endregion
    }
}
