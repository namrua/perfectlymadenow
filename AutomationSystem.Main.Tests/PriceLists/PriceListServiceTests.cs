using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.AppLogic;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.PriceLists
{
    public class PriceListServiceTests
    {
        private readonly Mock<IPriceListDatabaseLayer> priceListDbMock;
        private readonly Mock<IPriceListFactory> priceListFactoryMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IIdentityResolver> identityResolverMock;
        private readonly Mock<IEventDispatcher> eventDispatcherMock;

        public PriceListServiceTests()
        {
            priceListDbMock = new Mock<IPriceListDatabaseLayer>();
            priceListFactoryMock = new Mock<IPriceListFactory>();
            mainMapperMock = new Mock<IMainMapper>();
            identityResolverMock = new Mock<IIdentityResolver>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
        }

        #region GetPriceListMainPageModel() tests
        [Fact]
        public void GetPriceListMainPageModel_ExpectedIncludesPassToGetPriceLists()
        {
            // arrange
            priceListDbMock.Setup(e => e.GetPriceLists(It.IsAny<PriceListIncludes>())).Returns(new List<PriceList>());
            var service = CreateService();

            // act
            service.GetPriceListMainPageModel();

            // assert
            priceListDbMock.Verify(e => e.GetPriceLists(PriceListIncludes.PriceListType | PriceListIncludes.Currency), Times.Once);
        }

        [Fact]
        public void GetPriceListMainPageModel_PriceListTypesMocked_ReturnsPriceListTypes()
        {
            // arrange
            priceListDbMock.Setup(e => e.GetPriceLists(It.IsAny<PriceListIncludes>())).Returns(new List<PriceList>());
            var priceListTypes = new List<PriceListType>();
            priceListDbMock.Setup(e => e.GetPriceListTypes()).Returns(priceListTypes);
            var service = CreateService();

            // act
            var result = service.GetPriceListMainPageModel();

            // assert
            Assert.Same(priceListTypes, result.PriceListTypes);
        }

        [Fact]
        public void GetPriceListMainPageModel_PriceListsFromDb_IsMappedToResult()
        {
            // arrange
            var priceListOne = new PriceList();
            var priceListTwo = new PriceList();
            priceListDbMock.Setup(e => e.GetPriceLists(It.IsAny<PriceListIncludes>())).Returns(new List<PriceList> { priceListOne, priceListTwo });
            var priceListListItemOne = new PriceListListItem();
            var priceListListItemTwo = new PriceListListItem();
            mainMapperMock.SetupSequence(e => e.Map<PriceListListItem>(It.IsAny<PriceList>()))
                .Returns(priceListListItemOne)
                .Returns(priceListListItemTwo);
            var service = CreateService();

            // act
            var result = service.GetPriceListMainPageModel();

            // assert
            mainMapperMock.Verify(e => e.Map<PriceListListItem>(priceListOne), Times.Once);
            mainMapperMock.Verify(e => e.Map<PriceListListItem>(priceListTwo), Times.Once);
            Assert.Collection(
                result.Items,
                item => Assert.Equal(priceListListItemOne, item),
                item => Assert.Equal(priceListListItemTwo, item));

        }
        #endregion

        #region GetNewPriceListForEdit() tests
        [Fact]
        public void GetNewPriceListForEdit_ForGivenPriceListTypeId_PriceListForEditIsInitialized()
        {
            // arrange
            var forEdit = new PriceListForEdit();
            priceListFactoryMock.Setup(e => e.InitializePriceListForEdit(It.IsAny<PriceListTypeEnum>())).Returns(forEdit);
            var service = CreateService();

            // act
            var result = service.GetNewPriceListForEdit(PriceListTypeEnum.Lecture);

            // assert
            priceListFactoryMock.Verify(e => e.InitializePriceListForEdit(PriceListTypeEnum.Lecture), Times.Once);
            Assert.Same(forEdit, result);
        }

        [Fact]
        public void GetNewPriceListForEdit_PriceListTypeIdPassToPriceListFormAndCurrencySetToDefault()
        {
            // arrange
            priceListFactoryMock.Setup(e => e.InitializePriceListForEdit(It.IsAny<PriceListTypeEnum>())).Returns(new PriceListForEdit());
            var service = CreateService();

            // act
            var result = service.GetNewPriceListForEdit(PriceListTypeEnum.Lecture);

            // assert
            Assert.Equal(PriceListTypeEnum.Lecture, result.Form.PriceListTypeId);
            Assert.Equal(LocalisationInfo.DefaultCurrency, result.Form.CurrencyId);
        }
        #endregion

        #region GetPriceListForEditById() tests
        [Fact]
        public void GetPriceListForEditById_ExpectedIncludesPassToGetPriceListById()
        {
            // arrange
            var priceList = new PriceList
            {
                PriceListId = 1
            };
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(priceList);
            priceListFactoryMock.Setup(e => e.InitializePriceListForEdit(It.IsAny<PriceListTypeEnum>())).Returns(new PriceListForEdit());
            var service = CreateService();

            // act
            service.GetPriceListForEditById(1);

            // assert
            priceListDbMock.Verify(e => e.GetPriceListById(priceList.PriceListId, PriceListIncludes.PriceListItems), Times.Once);
        }

        [Fact]
        public void GetPriceListForEditById_PriceListIsNull_ThrowsArgumentException()
        {
            // arrange
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns((PriceList)null);
            var service = CreateService();

            // act & assert
            Assert.Throws<ArgumentException>(() => service.GetPriceListForEditById(1));
        }

        [Fact]
        public void GetPriceListForEditById_IsApprovedAndIsDiscardedSetToTrue_ThrowsInvalidOperationException()
        {
            // arrange
            var priceList = new PriceList
            {
                IsApprovded = true,
                IsDiscarded = true
            };
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(priceList);
            var service = CreateService();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => service.GetPriceListForEditById(1));
        }

        [Fact]
        public void GetPriceListForEditById_ForGivenPriceListTypeId_PriceListForEditIsInitialized()
        {
            // arrange
            var priceList = new PriceList
            {
                PriceListTypeId = PriceListTypeEnum.Lecture
            };
            var forEdit = new PriceListForEdit();
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(priceList);
            priceListFactoryMock.Setup(e => e.InitializePriceListForEdit(It.IsAny<PriceListTypeEnum>())).Returns(forEdit);
            mainMapperMock.Setup(e => e.Map<PriceListForm>(It.IsAny<PriceList>())).Returns(new PriceListForm());
            var service = CreateService();

            // act
            var result = service.GetPriceListForEditById(1);

            // assert
            priceListFactoryMock.Verify(e => e.InitializePriceListForEdit(PriceListTypeEnum.Lecture), Times.Once);
            Assert.Same(forEdit, result);
        }

        [Fact]
        public void GetPriceListForEditById_MockedPriceList_MappedToResultForm()
        {
            // arrange
            var priceList = new PriceList();
            var form = new PriceListForm();
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(priceList);
            priceListFactoryMock.Setup(e => e.InitializePriceListForEdit(It.IsAny<PriceListTypeEnum>())).Returns(new PriceListForEdit());
            mainMapperMock.Setup(e => e.Map<PriceListForm>(It.IsAny<PriceList>())).Returns(form);
            var service = CreateService();

            // act
            var result = service.GetPriceListForEditById(1);

            // assert
            Assert.Same(form, result.Form);
            mainMapperMock.Verify(e => e.Map<PriceListForm>(priceList), Times.Once);
        }
        #endregion

        #region GetPriceListForEditByForm() tests
        [Fact]
        public void GetPriceListForEditByForm_ForGivenPriceListForm_PriceListForEditIsInitialized()
        {
            // arrange
            var form = new PriceListForm
            {
                PriceListTypeId = PriceListTypeEnum.Lecture
            };
            var forEdit = new PriceListForEdit();
            priceListFactoryMock.Setup(e => e.InitializePriceListForEdit(It.IsAny<PriceListTypeEnum>())).Returns(forEdit);
            var service = CreateService();

            // act
            var result = service.GetPriceListForEditByForm(form);

            // assert
            priceListFactoryMock.Verify(e => e.InitializePriceListForEdit(PriceListTypeEnum.Lecture), Times.Once);
            Assert.Same(forEdit, result);
            Assert.Same(form, result.Form);
        }
        #endregion

        #region GetPriceListDetailById() tests
        [Fact]
        public void GetPriceListDetailById_ExpectedIncludesPassToGetPriceListById()
        {
            // arrange
            var priceList = new PriceList();
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(priceList);
            mainMapperMock.Setup(e => e.Map<PriceListDetail>(It.IsAny<PriceList>())).Returns(new PriceListDetail());
            var service = CreateService();

            // act
            service.GetPriceListDetailById(1);

            // assert
            priceListDbMock.Verify(e => e.GetPriceListById(
                1,
                PriceListIncludes.PriceListItemsRegistrationType
                | PriceListIncludes.PriceListType
                | PriceListIncludes.Currency),
                Times.Once);
        }

        [Fact]
        public void GetPriceListDetailById_PriceListIstNull_ThrowsArgumentException()
        {
            // arrange
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns((PriceList)null);
            var service = CreateService();

            // act & assert
            Assert.Throws<ArgumentException>(() => service.GetPriceListDetailById(1));
        }

        [Fact]
        public void GetPriceListDetailById_PriceListPassToMapper_ReturnsPriceListDetail()
        {
            // arrange
            var priceList = new PriceList();
            var detail = new PriceListDetail();
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(priceList);
            mainMapperMock.Setup(e => e.Map<PriceListDetail>(It.IsAny<PriceList>())).Returns(detail);
            var service = CreateService();

            // act
            var result = service.GetPriceListDetailById(1);

            // assert
            mainMapperMock.Verify(e => e.Map<PriceListDetail>(priceList), Times.Once);
            Assert.Same(detail, result);
        }

        [Fact]
        public void GetPriceListDetailById_CanDeleteIsTrue_ReturnsPriceListDetail()
        {
            // arrange
            priceListDbMock.Setup(e => e.GetPriceListById(It.IsAny<long>(), It.IsAny<PriceListIncludes>())).Returns(new PriceList());
            mainMapperMock.Setup(e => e.Map<PriceListDetail>(It.IsAny<PriceList>())).Returns(new PriceListDetail());
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<PriceListDeletingEvent>())).Returns(true);
            var service = CreateService();

            // act
            var result = service.GetPriceListDetailById(1);

            // assert
            Assert.True(result.CanDelete);
        }
        #endregion

        #region ValidatePriceList() tests
        [Fact]
        public void ValidatePriceList_PriceListFormPassToMapper()
        {
            // arrange
            var form = new PriceListForm();
            mainMapperMock.Setup(e => e.Map<List<PriceListItemForValidation>>(It.IsAny<PriceListForm>())).Returns(new List<PriceListItemForValidation>());
            var service = CreateService();

            // act
            service.ValidatePriceList(form);

            // assert
            mainMapperMock.Verify(e => e.Map<List<PriceListItemForValidation>>(form), Times.Once);
        }

        [Fact]
        public void ValidatePriceList_AllItemsHaveValue_ReturnsTrue()
        {
            // arrange
            var form = new PriceListForm
            {
                PriceListTypeId = PriceListTypeEnum.Lecture,
                LectureRegistration = 1
            };
            var items = new List<PriceListItemForValidation>
            {
                new PriceListItemForValidation
                {
                    Price = 1
                },
                new PriceListItemForValidation
                {
                    Price = 2
                },
                new PriceListItemForValidation
                {
                    Price = 3
                }
            };
            mainMapperMock.Setup(e => e.Map<List<PriceListItemForValidation>>(It.IsAny<PriceListForm>())).Returns(items);
            var service = CreateService();

            // act
            var result = service.ValidatePriceList(form);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePriceList_SomeItemHasNullValue_ReturnsFalse()
        {
            // arrange
            var form = new PriceListForm
            {
                PriceListTypeId = PriceListTypeEnum.Lecture,
                LectureRegistration = 1
            };
            var items = new List<PriceListItemForValidation>
            {
                new PriceListItemForValidation
                {
                    Price = 1
                },
                new PriceListItemForValidation
                {
                    Price = null
                },
                new PriceListItemForValidation
                {
                    Price = 3
                }
            };
            mainMapperMock.Setup(e => e.Map<List<PriceListItemForValidation>>(It.IsAny<PriceListForm>())).Returns(items);
            var service = CreateService();

            // act
            var result = service.ValidatePriceList(form);

            // assert
            Assert.False(result);
        }
        #endregion

        #region SavePriceList() tests
        [Fact]
        public void SavePriceList_PriceListFormPassToMapper()
        {
            // arrange
            var form = new PriceListForm();
            mainMapperMock.Setup(e => e.Map<PriceList>(It.IsAny<PriceListForm>())).Returns(new PriceList());
            var service = CreateService();

            // act
            service.SavePriceList(form);

            // assert
            mainMapperMock.Verify(e => e.Map<PriceList>(form), Times.Once);
        }

        [Fact]
        public void SavePriceList_NewPriceList_SavesPriceList()
        {
            // arrange
            var priceList = new PriceList
            {
                CurrencyId = LocalisationInfo.DefaultCurrency
            };
            mainMapperMock.Setup(e => e.Map<PriceList>(It.IsAny<PriceListForm>())).Returns(priceList);
            PriceList priceListToInsert = null;
            priceListDbMock.Setup(e => e.InsertPriceList(It.IsAny<PriceList>()))
                .Callback(new Action<PriceList>(pl => priceListToInsert = pl))
                .Returns(1);
            identityResolverMock.Setup(e => e.GetOwnerId()).Returns(1);
            var service = CreateService();

            // act
            var result = service.SavePriceList(new PriceListForm());

            // assert
            priceListDbMock.Verify(e => e.InsertPriceList(priceList), Times.Once);
            Assert.NotNull(priceListToInsert);
            Assert.Equal(LocalisationInfo.DefaultCurrency, priceListToInsert.CurrencyId);
            Assert.False(priceListToInsert.IsDiscarded);
            Assert.False(priceListToInsert.IsApprovded);
            Assert.Equal(1, result);
            identityResolverMock.Verify(e => e.GetOwnerId(), Times.Once);
        }

        [Fact]
        public void SavePriceList_PassPriceListToUpdatePriceList_SavesPriceList()
        {
            // arrange
            var priceList = new PriceList
            {
                PriceListId = 1,
            };
            mainMapperMock.Setup(e => e.Map<PriceList>(It.IsAny<PriceListForm>())).Returns(priceList);
            var service = CreateService();

            // 
            var result = service.SavePriceList(new PriceListForm());

            // assert
            priceListDbMock.Verify(e => e.UpdatePriceList(priceList, It.IsAny<PriceListOperationOption>()), Times.Once);
            Assert.Equal(1, result);
        }
        #endregion

        #region ApprovePriceList() tests
        [Fact]
        public void ApprovePriceList_PassPriceListIdToApprovedPriceList()
        {
            // arrange
            var service = CreateService();

            // act
            service.ApprovePriceList(1);

            // assert
            priceListDbMock.Verify(e => e.ApprovePriceList(1), Times.Once);
        }
        #endregion

        #region DiscardPriceList() tests
        [Fact]
        public void DiscardPriceList_PassPriceListIdToDiscardPriceList()
        {
            // arrange
            var service = CreateService();

            // act
            service.DiscardPriceList(1);

            // assert
            priceListDbMock.Verify(e => e.DiscardPriceList(1), Times.Once);
        }
        #endregion

        #region DeletePriceList() tests
        [Fact]
        public void DeletePriceList_CanDeletePriceListIsFalse_ThrowsInvalidOperationException()
        {
            // arrange
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<PriceListDeletingEvent>())).Returns(false);
            var service = CreateService();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => service.DeletePriceList(1));
        }

        [Fact]
        public void DeletePriceList_PassPriceListIdToDeletePriceList()
        {
            // arrange
            eventDispatcherMock.Setup(e => e.Check(It.IsAny<PriceListDeletingEvent>())).Returns(true);
            var service = CreateService();

            // act
            service.DeletePriceList(1);

            // assert
            priceListDbMock.Verify(e => e.DeletePriceList(1), Times.Once);
        }
        #endregion

        #region private methods
        private PriceListService CreateService()
        {
            return new PriceListService(
                priceListDbMock.Object,
                priceListFactoryMock.Object,
                mainMapperMock.Object,
                identityResolverMock.Object,
                eventDispatcherMock.Object);
        }
        #endregion
    }
}
