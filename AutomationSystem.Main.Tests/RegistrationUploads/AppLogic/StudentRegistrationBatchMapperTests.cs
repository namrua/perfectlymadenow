using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Model;
using Moq;
using System;
using System.Collections.Generic;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using Xunit;

namespace AutomationSystem.Main.Tests.RegistrationUploads.AppLogic
{
    public class StudentRegistrationBatchMapperTests
    {
        private readonly Mock<IBatchUploadDatabaseLayer> batchUploadDbMock;
        private readonly Mock<ICoreMapper> coreMapperMock;
        private readonly Mock<IEnumDatabaseLayer> enumDbMock;

        public StudentRegistrationBatchMapperTests()
        {
            batchUploadDbMock = new Mock<IBatchUploadDatabaseLayer>();
            coreMapperMock = new Mock<ICoreMapper>();
            enumDbMock = new Mock<IEnumDatabaseLayer>();
        }

        #region MapToBatchUploadDetail() tests

        [Fact]
        public void MapToBatchUploadDetail_BatchUploadItemsAreNull_ThrowsInvalidOperationException()
        {
            // arrange
            var batchUpload = new BatchUpload
            {
                BatchUploadItems = null
            };
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.MapToBatchUploadDetail(batchUpload));
        }

        [Fact]
        public void MapToBatchUploadDetail_BatchUpload_ReturnsBatchUploadDetailWithMapPropertiesAndFilteredOperationTypes()
        {
            // arrange
            var batchUpload = new BatchUpload
            {
                BatchUploadItems = new List<BatchUploadItem>
                {
                    CreateBatchUploadItem(1)
                }
            };
            var listItem = new BatchUploadListItem();
            coreMapperMock.Setup(e => e.Map<BatchUploadListItem>(It.IsAny<BatchUpload>())).Returns(listItem);
            batchUploadDbMock.Setup(e => e.GetBatchUploadOperationTypes()).Returns(CreateOperationTypes);
            enumDbMock.Setup(e => e.GetItemById(It.IsAny<EnumTypeEnum>(), It.IsAny<int>())).Returns(new EnumItem(1));
            var mapper = CreateMapper();

            // act
            var result = mapper.MapToBatchUploadDetail(batchUpload);

            // assert
            Assert.Same(listItem, result.BatchUpload);
            Assert.Collection(result.Items,
                item => Assert.Equal(1, item.BatchUploadItemId));
            Assert.Collection(result.BatchUploadOperationTypes,
                item => Assert.Equal(BatchUploadOperationTypeEnum.New, item.BatchUploadOperationTypeId),
                item => Assert.Equal(BatchUploadOperationTypeEnum.Ignore, item.BatchUploadOperationTypeId));
            coreMapperMock.Verify(e => e.Map<BatchUploadListItem>(batchUpload), Times.Once);
            enumDbMock.Verify(e => e.GetItemById(EnumTypeEnum.Country, 1), Times.Once);
        }

        [Fact]
        public void MapToBatchUploadDetail_BatchUpload_IsMappedToBatchUploadListItem()
        {
            // arrange
            var batchUpload = new BatchUpload
            {
                BatchUploadItems = new List<BatchUploadItem>
                {
                    CreateBatchUploadItem(4)
                }
            };
            coreMapperMock.Setup(e => e.Map<BatchUploadListItem>(It.IsAny<BatchUpload>())).Returns(new BatchUploadListItem());
            batchUploadDbMock.Setup(e => e.GetBatchUploadOperationTypes()).Returns(CreateOperationTypes);
            enumDbMock.Setup(e => e.GetItemById(It.IsAny<EnumTypeEnum>(), It.IsAny<int>())).Returns(new EnumItem(1));
            var mapper = CreateMapper();

            // act
            var result = mapper.MapToBatchUploadDetail(batchUpload);

            // assert
            Assert.Collection(result.Items,
                item =>
                {
                    Assert.Equal(4, item.BatchUploadItemId);
                    Assert.Equal(BatchUploadOperationTypeEnum.New, item.BatchUploadOperationTypeId);
                    Assert.Equal("New", item.BatchUploadOperationType);
                    Assert.Equal(10, item.EntityId);
                    Assert.Equal(20, item.PairEntityId);
                    Assert.True(item.IsValid);
                });
        }

        #endregion

        #region MapToStudentForm() tests

        [Fact]
        public void MapToStudentForm_BatchUploadItem_ReturnsRegistrationStudentForm()
        {
            // arrange
            var item = CreateBatchUploadItem(5);
            var mapper = CreateMapper();

            // act
            var result = mapper.MapToStudentForm(item);

            // assert
            Assert.Equal("FirstName", result.Address.FirstName);
            Assert.Equal("LastName", result.Address.LastName);
            Assert.Equal("Street", result.Address.Street);
            Assert.Equal("Street2", result.Address.Street2);
            Assert.Equal("City", result.Address.City);
            Assert.Equal("State", result.Address.State);
            Assert.Equal("ZipCode", result.Address.ZipCode);
            Assert.Equal("Phone", result.Phone);
            Assert.Equal("Email", result.Email);
            Assert.Equal(CountryEnum.AF, result.Address.CountryId);
        }

        #endregion

        #region UpdateBatchUploadItem() tests

        [Fact]
        public void UpdateBatchUploadItem_RegistrationStudentFormAndBatchUploadItem_FieldsAreSet()
        {
            var form = new RegistrationStudentForm
            {
                Address = new AddressForm
                {
                    FirstName = "First",
                    LastName = "Last",
                    Street = "Cejl",
                    Street2 = null,
                    City = "Brno",
                    State = "Cz",
                    ZipCode = "60001",
                    CountryId = CountryEnum.CZ,
                },
                Phone = "123456789",
                Email = "email@email.com"
            };
            var item = CreateBatchUploadItem(3);
            item.IsValid = false;
            var mapper = CreateMapper();

            // act
            mapper.UpdateBatchUploadItem(form, item);

            // assert
            Assert.True(item.IsValid);
            Assert.Collection(item.BatchUploadFields,
                field => Assert.Equal("Last", field.Value),
                field => Assert.Equal("First", field.Value),
                field => Assert.Equal("Cejl", field.Value),
                field => Assert.Null(field.Value),
                field => Assert.Equal("Brno", field.Value),
                field => Assert.Equal("Cz", field.Value),
                field => Assert.Equal("60001", field.Value),
                field => Assert.Equal(((int)CountryEnum.CZ).ToString(), field.Value),
                field => Assert.Equal("123456789", field.Value),
                field => Assert.Equal("email@email.com", field.Value)
                );
        }

        #endregion

        #region private methods

        private StudentRegistrationBatchMapper CreateMapper()
        {
            return new StudentRegistrationBatchMapper(enumDbMock.Object, batchUploadDbMock.Object, coreMapperMock.Object);
        }

        private BatchUploadField CreateField(int order, string value)
        {
            return new BatchUploadField
            {
                Order = order,
                Value = value
            };
        }

        private List<BatchUploadOperationType> CreateOperationTypes()
        {
            return new List<BatchUploadOperationType>
            {
                new BatchUploadOperationType
                {
                    BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.New,
                    Name = "New"
                },
                new BatchUploadOperationType
                {
                    BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.Update,
                    Name = "Update"
                },
                new BatchUploadOperationType
                {
                    BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.Ignore,
                    Name = "Ignore"
                }
            };
        }

        private List<BatchUploadField> CreateFields()
        {
            return new List<BatchUploadField>
            {
                CreateField(0, "LastName"), CreateField(1, "FirstName"), CreateField(2, "Street"), CreateField(3, "Street2"), CreateField(4, "City"),
                CreateField(5, "State"), CreateField(6, "ZipCode"), CreateField(7, "1"), CreateField(8, "Phone"), CreateField(9, "Email")
            };
        }

        private BatchUploadItem CreateBatchUploadItem(int itemId)
        {
            return new BatchUploadItem
            {
                BatchUploadItemId = itemId,
                BatchUploadFields = CreateFields(),
                BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.New,
                BatchUploadOperationType = new BatchUploadOperationType
                {
                    Description = "New"
                },
                EntityId = 10,
                PairEntityId = 20,
                IsValid = true
            };
        }
        #endregion
    }
}
