using System;
using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;
using AutomationSystem.Main.Core.Classes.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Model;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.MappingProfiles
{
    public class ClassPreferenceProfileTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public ClassPreferenceProfileTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
            AddRegistrationColorSchemeTypes();
            AddClassExpenseTypes();
        }

        #region CreateMap<RegistrationColorSchemeEnum, string>() tests
        [Theory]
        [InlineData(ClassExpenseTypeEnum.Custom, "Custom")]
        [InlineData(ClassExpenseTypeEnum.FoiRoyaltyFee, "FOI Royalty Fee")]
        [InlineData(ClassExpenseTypeEnum.PayPalFeeWwaLecture, null)]
        public void Map_ClassExpenseTypeEnum_ReturnsExpectedString(ClassExpenseTypeEnum expenseType, string expectedValue)
        {
            // arrange
            var mapper = CreateMapper();

            // act
            var actualValue = mapper.Map<string>(expenseType);

            // assert
            Assert.Equal(expectedValue, actualValue);
        }
        #endregion

        #region CreateMap<ClassPreference, ClassPreferenceForm>() tests
        [Fact]
        public void Map_ClassPreference_ReturnsClassPreferenceForm()
        {
            // arrange
            var mapper = CreateMapper();
            var preference = CreateClassPreference();

            // act
            var classPreferenceForm = mapper.Map<ClassPreferenceForm>(preference);

            // assert
            Assert.Equal("Url", classPreferenceForm.HomepageUrl);
            Assert.Equal(1, classPreferenceForm.OriginHeaderPictureId);
            Assert.Equal(RegistrationColorSchemeEnum.Limet, classPreferenceForm.RegistrationColorSchemeId);
            Assert.True(classPreferenceForm.SendCertificatesByEmail);
            Assert.Equal("VenueName", classPreferenceForm.VenueName);
            Assert.Equal("LocationCode", classPreferenceForm.LocationCode);
            Assert.False(classPreferenceForm.RemoveHeaderPicture);
            Assert.Equal(1, classPreferenceForm.LocationInfoId);
            Assert.Equal(CurrencyEnum.MXN, classPreferenceForm.CurrencyId);
        }
        #endregion

        #region CreateMap<ClassPreference, ClassPreferenceDetail>() tests
        [Fact]
        public void Map_ClassPreferenceExpensesIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var preference = CreateClassPreference();
            preference.ClassPreferenceExpenses = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<ClassPreferenceDetail>(preference));
        }

        [Fact]
        public void Map_LocationInfoIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var preference = CreateClassPreference();
            preference.LocationInfo = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<ClassPreferenceDetail>(preference));
        }

        [Fact]
        public void Map_LocationInfoAddressIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var preference = CreateClassPreference();
            preference.LocationInfo.Address = null;
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<ClassPreferenceDetail>(preference));
        }

        [Fact]
        public void Map_CurrencyIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var preference = CreateClassPreference();
            preference.Currency = null;
            var mapper = CreateMapper();

            // act
            Assert.Throws<InvalidOperationException>(() => mapper.Map<ClassPreferenceDetail>(preference));
        }

        [Fact]
        public void Map_ClassPreference_ReturnsClassPreferenceDetail()
        {
            // arrange
            var preference = CreateClassPreference();
            preference.ClassPreferenceExpenses = CreateClassPreferenceExpenses();
            var mapper = CreateMapper();

            // act
            var classPreferenceDetail = mapper.Map<ClassPreferenceDetail>(preference);

            // assert
            Assert.Equal("Url", classPreferenceDetail.HomepageUrl);
            Assert.Equal(1, classPreferenceDetail.HeaderPictureId);
            Assert.Equal(RegistrationColorSchemeEnum.Limet, classPreferenceDetail.RegistrationColorSchemeId);
            Assert.Equal("Limet", classPreferenceDetail.RegistrationColorScheme);
            Assert.True(classPreferenceDetail.SendCertificatesByEmail);
            Assert.Equal("VenueName", classPreferenceDetail.VenueName);
            Assert.Equal("LocationCode", classPreferenceDetail.LocationCode);
            Assert.Equal("Mexican peso (MXN)", classPreferenceDetail.Currency);
            Assert.Equal("MXN", classPreferenceDetail.CurrencyCode);
            Assert.Collection(classPreferenceDetail.Expenses,
                item => Assert.Equal(1, item.Order),
                item => Assert.Equal(2, item.Order),
                item => Assert.Equal(3, item.Order));
            Assert.Equal("firstName lastName", classPreferenceDetail.LocationInfo);
        }

        [Fact]
        public void Map_LocationInfoIdIsNull_SetLocationInfoToNull()
        {
            // arrange
            var preference = CreateClassPreference();
            preference.LocationInfoId = null;
            preference.LocationInfo = null;
            var mapper = CreateMapper();

            // act
            var classPreferenceDetail = mapper.Map<ClassPreferenceDetail>(preference);

            // assert
            Assert.NotNull(classPreferenceDetail);
            Assert.Null(classPreferenceDetail.LocationInfo);
        }
        #endregion

        #region CreateMap<ClassPreferenceForm, ClassPreference>() tests
        [Fact]
        public void Map_ClassPreferenceForm_ReturnsClassPreference()
        {
            // arrange
            var classPreferenceForm = new ClassPreferenceForm
            {
                HomepageUrl = "Url",
                RegistrationColorSchemeId = RegistrationColorSchemeEnum.Limet,
                SendCertificatesByEmail = true,
                VenueName = "VenueName",
                LocationCode = "LocationCode",
                OriginHeaderPictureId = 1,
                RemoveHeaderPicture = false,
                CurrencyId = CurrencyEnum.MXN
            };
            var mapper = CreateMapper();

            // act
            var classPreference = mapper.Map<ClassPreference>(classPreferenceForm);

            // assert
            Assert.Equal("http://Url", classPreference.HomepageUrl);
            Assert.Equal(RegistrationColorSchemeEnum.Limet, classPreference.RegistrationColorSchemeId);
            Assert.True(classPreference.SendCertificatesByEmail);
            Assert.Equal("VenueName", classPreference.VenueName);
            Assert.Equal("LocationCode", classPreference.LocationCode);
            Assert.Equal(1, classPreference.HeaderPictureId);
            Assert.Equal(CurrencyEnum.MXN, classPreference.CurrencyId);
        }
        #endregion

        #region private methods
        private void AddRegistrationColorSchemeTypes()
        {
            var colorSchemes = new List<RegistrationColorScheme>()
            {
                new RegistrationColorScheme
                {
                    RegistrationColorSchemeId = RegistrationColorSchemeEnum.Limet,
                    Description = "Limet",
                    Name = "Name",
                },
                new RegistrationColorScheme
                {
                    RegistrationColorSchemeId = RegistrationColorSchemeEnum.Ocean,
                    Description = "Ocean",
                    Name = "Name",
                }
            };

            classDbMock.Setup(x => x.GetRegistrationColorSchemes()).Returns(colorSchemes);
        }
        
        private void AddClassExpenseTypes()
        {
            var expenseTypes = new List<ClassExpenseType>()
            {
                new ClassExpenseType()
                {
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Description = "Custom"
                },
                new ClassExpenseType()
                {
                    ClassExpenseTypeId = ClassExpenseTypeEnum.FoiRoyaltyFee,
                    Description = "FOI Royalty Fee"
                }
            };
            classDbMock.Setup(x => x.GetClassExpenseTypes()).Returns(expenseTypes);
        }

        private Mapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ClassPreferenceProfile(classDbMock.Object));
                cfg.AddProfile(new ClassExpenseProfile(classDbMock.Object));
            });
            return new Mapper(mapperConfiguration);
        }

        private ClassPreference CreateClassPreference()
        {
            return new ClassPreference
            {
                HomepageUrl = "Url",
                HeaderPictureId = 1,
                RegistrationColorSchemeId = RegistrationColorSchemeEnum.Limet,
                SendCertificatesByEmail = true,
                VenueName = "VenueName",
                LocationCode = "LocationCode",
                LocationInfoId = 1,
                CurrencyId = CurrencyEnum.MXN,
                Currency = new Currency
                {
                    Description = "Mexican peso",
                    Name = "MXN"

                },
                LocationInfo = new Person()
                {
                    Address = new Address()
                    {
                        FirstName = "firstName",
                        LastName = "lastName"
                    }
                }
            };
        }
        
        private List<ClassPreferenceExpense> CreateClassPreferenceExpenses()
        {
            return new List<ClassPreferenceExpense>()
            {
                new ClassPreferenceExpense
                {
                    Order = 1,
                    Text = "first",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 1
                },
                new ClassPreferenceExpense
                {
                    Order = 2,
                    Text = "second",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 2
                },
                new ClassPreferenceExpense
                {
                    Order = 3,
                    Text = "third",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 3
                }
            };
        }
        #endregion
    }
}
