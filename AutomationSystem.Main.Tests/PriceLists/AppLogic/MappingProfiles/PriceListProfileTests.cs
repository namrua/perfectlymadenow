using System;
using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models;
using AutomationSystem.Main.Core.PriceLists.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.PriceLists.AppLogic.MappingProfiles
{
    public class PriceListProfileTests
    {
        private readonly DateTime approved = new DateTime(2020, 10, 30);
        private readonly DateTime discarded = new DateTime(2020, 10, 30);
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;
        private readonly Mock<IPriceListTypeResolver> priceListTypeResolverMock;

        public PriceListProfileTests()
        {
            enumDbMock = new Mock<IEnumDatabaseLayer>();
            priceListTypeResolverMock = new Mock<IPriceListTypeResolver>();
            AddRegistrationTypeEnum();
        }

        #region CreateMap<PriceListForm, PriceList>() tests
        [Fact]
        public void Map_PriceListForm_ReturnsPriceList()
        {
            // arrange
            var classRegistrationTypeEnums = CreateClassEnums();
            priceListTypeResolverMock.Setup(e => e.GetRegistrationTypesForPriceList(It.IsAny<PriceListTypeEnum>())).Returns(classRegistrationTypeEnums);
            var form = new PriceListForm()
            {
                Name = "Name",
                CurrencyId = CurrencyEnum.USD,
                PriceListId = 1,
                PriceListTypeId = PriceListTypeEnum.Class,
                NewAdult = 1,
                NewAdultWeekOfClass = 2,
                NewChild = 3,
                ReviewAdult = 4,
                ReviewChild = 5,
                WWA = 6
            };
            var mapper = CreateMapper();

            // act
            var list = mapper.Map<PriceList>(form);

            // assert
            Assert.Equal(1, list.PriceListId);
            Assert.Equal("Name", list.Name);
            Assert.Equal(CurrencyEnum.USD, list.CurrencyId);
            Assert.Equal(PriceListTypeEnum.Class, list.PriceListTypeId);
            Assert.Collection(list.PriceListItems,
                item =>
                {
                    Assert.Equal(RegistrationTypeEnum.NewAdult, item.RegistrationTypeId);
                    Assert.Equal(1, item.Price);
                    Assert.Equal(1, item.PriceListId);
                },
                item =>
                {
                    Assert.Equal(RegistrationTypeEnum.NewAdultWeekOfClass, item.RegistrationTypeId);
                    Assert.Equal(2, item.Price);
                    Assert.Equal(1, item.PriceListId);
                },
                item =>
                {
                    Assert.Equal(RegistrationTypeEnum.NewChild, item.RegistrationTypeId);
                    Assert.Equal(3, item.Price);
                    Assert.Equal(1, item.PriceListId);
                },
                item =>
                {
                    Assert.Equal(RegistrationTypeEnum.ReviewAdult, item.RegistrationTypeId);
                    Assert.Equal(4, item.Price);
                    Assert.Equal(1, item.PriceListId);
                },
                item =>
                {
                    Assert.Equal(RegistrationTypeEnum.ReviewChild, item.RegistrationTypeId);
                    Assert.Equal(5, item.Price);
                    Assert.Equal(1, item.PriceListId);
                },
                item =>
                {
                    Assert.Equal(RegistrationTypeEnum.WWA, item.RegistrationTypeId);
                    Assert.Equal(6, item.Price);
                    Assert.Equal(1, item.PriceListId);
                });
            priceListTypeResolverMock.Verify(e => e.GetRegistrationTypesForPriceList(PriceListTypeEnum.Class), Times.Once);
        }

        [Fact]
        public void Map_PriceIsNull_ReturnsPriceListWithDefaultPrice()
        {
            // arrange
            var lectureRegistrationTypeEnums = CreateLectureEnums();
            priceListTypeResolverMock.Setup(e => e.GetRegistrationTypesForPriceList(It.IsAny<PriceListTypeEnum>())).Returns(lectureRegistrationTypeEnums);
            var form = new PriceListForm()
            {
                PriceListTypeId = PriceListTypeEnum.Lecture,
                LectureRegistration = null
            };
            var mapper = CreateMapper();

            // act
            var list = mapper.Map<PriceList>(form);

            // assert
            Assert.Collection(list.PriceListItems,
                item => Assert.Equal(0, item.Price));
            priceListTypeResolverMock.Verify(e => e.GetRegistrationTypesForPriceList(PriceListTypeEnum.Lecture), Times.Once);
        }
        #endregion

        #region CreateMap<PriceListItem, PriceListItemDetail>() tests
        [Fact]
        public void MapPriceListItemToPriceListItemDetail_RegistrationTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var item = CreatePriceListItem();
            item.RegistrationType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<PriceListItemDetail>(item));
        }

        [Fact]
        public void Map_PriceListItem_ReturnsPriceListItemDetail()
        {
            // arrange
            var item = CreatePriceListItem();
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<PriceListItemDetail>(item);

            // arrange
            Assert.Equal(RegistrationTypeEnum.NewChild, detail.RegistrationTypeId);
            Assert.Equal(1, detail.Price);
            Assert.Equal("Description", detail.Name);
        }
        #endregion

        #region  CreateMap<PriceList, PriceListDetail>() tests
        [Fact]
        public void MapPriceListToPriceListDetail_PriceListItemsAreNull_ThrowsInvalidOperationException()
        {
            // arrange
            var list = CreatePriceList();
            list.PriceListItems = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<PriceListDetail>(list));
        }

        [Fact]
        public void MapPriceListToPriceListDetail_PriceListTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var list = CreatePriceList();
            list.PriceListType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<PriceListDetail>(list));
        }

        [Fact]
        public void MapPriceListToPriceListDetail_CurrencyIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var list = CreatePriceList();
            list.Currency =  null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<PriceListDetail>(list));
        }

        [Fact]
        public void Map_PriceList_ReturnsPriceListDetail()
        {
            // arrange
            var list = CreatePriceList();
            var mapper = CreateMapper();

            // act
            var detail = mapper.Map<PriceListDetail>(list);

            // asseert
            Assert.Equal("Name", detail.Name);
            Assert.Equal("Description", detail.PriceListType);
            Assert.Equal("US Dollar (USD)", detail.Currency);
            Assert.Equal("USD", detail.CurrencyCode);
            Assert.Equal(approved, detail.Approved);
            Assert.Equal(discarded, detail.Discarded);
            Assert.Equal(PriceListState.Approved, detail.State);
            Assert.Collection(detail.PriceListItems,
                item => Assert.Equal(1, item.Price),
                item => Assert.Equal(2, item.Price));
        }
        #endregion

        #region CreateMap<PriceList, PriceListListItem>() tests
        [Fact]
        public void MapPriceListToPriceListListItem_PriceListTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var list = CreatePriceList();
            list.PriceListType = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<PriceListListItem>(list));
        }

        [Fact]
        public void MapPriceListToPriceListListItem_CurrencyIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var list = CreatePriceList();
            list.Currency = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<PriceListListItem>(list));
        }

        [Fact]
        public void Map_PriceList_ReturnsPriceListListItem()
        {
            // arrange
            var list = CreatePriceList();
            var mapper = CreateMapper();

            // act
            var item = mapper.Map<PriceListListItem>(list);

            // assert
            Assert.Equal("Name", item.Name);
            Assert.Equal(1, item.PriceListId);
            Assert.Equal("USD", item.CurrencyCode);
            Assert.Equal("Description", item.PriceListType);
        }

        [Theory]
        [InlineData(false, false, PriceListState.New)]
        [InlineData(true, true, PriceListState.Discard)]
        [InlineData(true, false, PriceListState.Approved)]
        [InlineData(false, true, PriceListState.Discard)]
        public void Map_SpecifiedIsApprovedAndIsDiscard_ReturnsExpectedPriceListState(bool isApproved, bool isDiscarded, PriceListState expectedState)
        {
            // arrange
            var mapper = CreateMapper();
            var list = new PriceList
            {
                PriceListType = new PriceListType(),
                Currency = new Currency(),
                IsApprovded = isApproved,
                IsDiscarded = isDiscarded
            };

            // act
            var actualState = mapper.Map<PriceListListItem>(list);

            // assert
            Assert.Equal(expectedState, actualState.State);
        }
        #endregion

        #region CreateMap<PriceList, PriceListForm>() tests
        [Fact]
        public void MapPriceListToPriceListForm_PriceListItemsAreNull_ThrowsInvalidOperationException()
        {
            // arrange
            var list = CreatePriceList();
            list.PriceListItems = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<PriceListForm>(list));
        }

        [Fact]
        public void Map_PriceList_ReturnsPriceListForm()
        {
            // arrange
            var list = CreatePriceList();
            var mapper = CreateMapper();

            // act
            var form = mapper.Map<PriceListForm>(list);

            // assert
            Assert.Equal("Name", form.Name);
            Assert.Equal(1, form.PriceListId);
            Assert.Equal(CurrencyEnum.USD, form.CurrencyId);
            Assert.Equal(PriceListTypeEnum.Class, form.PriceListTypeId);
        }

        [Theory]
        [InlineData(RegistrationTypeEnum.NewAdult, nameof(PriceListForm.NewAdult))]
        [InlineData(RegistrationTypeEnum.NewAdultWeekOfClass, nameof(PriceListForm.NewAdultWeekOfClass))]
        [InlineData(RegistrationTypeEnum.NewChild, nameof(PriceListForm.NewChild))]
        [InlineData(RegistrationTypeEnum.ReviewAdult, nameof(PriceListForm.ReviewAdult))]
        [InlineData(RegistrationTypeEnum.ReviewChild, nameof(PriceListForm.ReviewChild))]
        [InlineData(RegistrationTypeEnum.WWA, nameof(PriceListForm.WWA))]
        [InlineData(RegistrationTypeEnum.LectureRegistration, nameof(PriceListForm.LectureRegistration))]
        [InlineData(RegistrationTypeEnum.MaterialRegistration, nameof(PriceListForm.MaterialRegistration))]
        public void Map_SpecifiedPriceListItemsPrice_IsSetToExpectedFormProperty(RegistrationTypeEnum type, string formPricePropertyName)
        {
            // arrange
            var mapper = CreateMapper();
            var list = new PriceList
            {
                PriceListType = new PriceListType(),
                PriceListTypeId = PriceListTypeEnum.Class,
                PriceListItems = new List<PriceListItem>
                {
                    new PriceListItem
                    {
                        Price = 1.0M,
                        RegistrationTypeId = type,
                    }
                }
            };

            // act
            var form = mapper.Map<PriceListForm>(list);

            // assert
            var actualPrice = (decimal)form.GetType().GetProperty(formPricePropertyName).GetValue(form);
            Assert.Equal(1.0M, actualPrice);
        }
        #endregion

        #region CreateMap<PriceListForm, List<PriceListItemForValidation>>() tests
        [Fact]
        public void Map_PriceListForm_ReturnsPriceListItemsForValidation()
        {
            // arrange
            var classRegistrationTypeEnums = CreateClassEnums();
            priceListTypeResolverMock.Setup(e => e.GetRegistrationTypesForPriceList(It.IsAny<PriceListTypeEnum>())).Returns(classRegistrationTypeEnums);
            var form = new PriceListForm
            {
                Name = "Name",
                PriceListId = 1,
                PriceListTypeId = PriceListTypeEnum.Class,
                NewAdult = 1,
                NewAdultWeekOfClass = 2,
                NewChild = 3,
                ReviewAdult = 4,
                ReviewChild = 5,
                WWA = 6
            };
            var mapper = CreateMapper();

            // act
            var items = mapper.Map<List<PriceListItemForValidation>>(form);

            // assert
            Assert.Collection(items,
                item => Assert.Equal(1, item.Price),
                item => Assert.Equal(2, item.Price),
                item => Assert.Equal(3, item.Price),
                item => Assert.Equal(4, item.Price),
                item => Assert.Equal(5, item.Price),
                item => Assert.Equal(6, item.Price));
            priceListTypeResolverMock.Verify(e => e.GetRegistrationTypesForPriceList(PriceListTypeEnum.Class), Times.Once);
        }

        [Fact]
        public void Map_PriceIsNull_ReturnsPriceListItemsForValidationWithNullValue()
        {
            // arrange
            var lectureRegistrationTypeEnums = CreateLectureEnums();
            priceListTypeResolverMock.Setup(e => e.GetRegistrationTypesForPriceList(It.IsAny<PriceListTypeEnum>())).Returns(lectureRegistrationTypeEnums);
            var form = new PriceListForm
            {
                PriceListTypeId = PriceListTypeEnum.Lecture,
                LectureRegistration = null
            };
            var mapper = CreateMapper();

            // act
            var items = mapper.Map<List<PriceListItemForValidation>>(form);

            // assert
            Assert.Collection(items,
                item => Assert.Null(item.Price));
            priceListTypeResolverMock.Verify(e => e.GetRegistrationTypesForPriceList(PriceListTypeEnum.Lecture), Times.Once);
        }
        #endregion

        #region private methods
        private void AddRegistrationTypeEnum()
        {
            var enumTypesEnum = new List<IEnumItem>
            {
                new RegistrationType
                {
                    RegistrationTypeId = RegistrationTypeEnum.ApprovedGuest,
                    Name = "ApprovedGuest",
                    Description = "Approved Guest"
                },
                new RegistrationType
                {
                    RegistrationTypeId = RegistrationTypeEnum.NewChild,
                    Name = "NewChild",
                    Description = "New Child"
                }
            };

            enumDbMock.Setup(x => x.GetItemsByFilter(It.IsAny<EnumTypeEnum>(), It.IsAny<EnumItemFilter>())).Returns(enumTypesEnum);
        }

        private Mapper CreateMapper()
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PriceListProfile(priceListTypeResolverMock.Object));
            });
            return new Mapper(mapperCfg);
        }

        private PriceList CreatePriceList()
        {
            return new PriceList
            {
                Name = "Name",
                PriceListId = 1,
                PriceListType = new PriceListType
                {
                    Description = "Description"
                },
                PriceListTypeId = PriceListTypeEnum.Class,
                CurrencyId = CurrencyEnum.USD,
                Currency = new Currency
                {
                    Name = "USD",
                    Description = "US Dollar"
                },
                Approved = approved,
                Discarded = discarded,
                IsApprovded = true,
                IsDiscarded = false,
                PriceListItems = CreatePriceListItems()
            };
        }

        private List<PriceListItem> CreatePriceListItems()
        {
            return new List<PriceListItem>
            {
                CreatePriceListItem(1, RegistrationTypeEnum.NewChild),
                CreatePriceListItem(2, RegistrationTypeEnum.NewAdult)
            };
        }
        
        private PriceListItem CreatePriceListItem(decimal price = 1, RegistrationTypeEnum type = RegistrationTypeEnum.NewChild)
        {
            return new PriceListItem()
            {
                Price = price,
                RegistrationTypeId = type,
                RegistrationType = new RegistrationType
                {
                    Description = "Description"
                }
            };
        }

        private HashSet<RegistrationTypeEnum> CreateClassEnums()
        {
            return new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.NewAdult,
                RegistrationTypeEnum.NewAdultWeekOfClass,
                RegistrationTypeEnum.NewChild,
                RegistrationTypeEnum.ReviewAdult,
                RegistrationTypeEnum.ReviewChild,
                RegistrationTypeEnum.WWA
            };
        }

        private HashSet<RegistrationTypeEnum> CreateLectureEnums()
        {
            return new HashSet<RegistrationTypeEnum>
            {
                RegistrationTypeEnum.LectureRegistration
            };
        }
        #endregion
    }
}
