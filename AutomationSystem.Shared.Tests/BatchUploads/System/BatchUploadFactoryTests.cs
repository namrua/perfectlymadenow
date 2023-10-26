using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Core.BatchUploads.System;
using Moq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace AutomationSystem.Shared.Tests.BatchUploads.System
{
    public class BatchUploadFactoryTests
    {
        private const EntityTypeEnum ParentEntityTypeId = EntityTypeEnum.MainClass;
        private const EntityTypeEnum UploadedEntityTypeId = EntityTypeEnum.MainClassRegistration;
        private const string JsonParameter = "JsonParameter";

        private readonly Mock<IBatchUploadValueResolver> batchUploadValueResolverMock;

        public BatchUploadFactoryTests()
        {
            batchUploadValueResolverMock = new Mock<IBatchUploadValueResolver>();
            batchUploadValueResolverMock.SetupSequence(e => e.GetValues(It.IsAny<string[]>()))
                .Returns(new[] {"dataA", "secondDataA"})
                .Returns(new[] { "dataB", "secondDataB" });
        }

        #region CreateBatchUpload() tests

        [Fact]
        public void CreateBatchUpload_ForGivenParameters_ReturnsBatchUploadWithSetValues()
        {
            // arrange
            var form = CreateForm();
            var data = CreateData();
            var factory = CreateFactory();

            // act
            var result = factory.CreateBatchUpload(data, form, ParentEntityTypeId, UploadedEntityTypeId, batchUploadValueResolverMock.Object, JsonParameter);

            // assert
            Assert.Equal(BatchUploadTypeEnum.StudentRegistrationExcel, result.BatchUploadTypeId);
            Assert.Equal(BatchUploadStateEnum.InUploading, result.BatchUploadStateId);
            Assert.Equal("Name", result.Name);
            Assert.Equal(ParentEntityTypeId, result.ParentEntityTypeId);
            Assert.Equal(UploadedEntityTypeId, result.UploadedEntityTypeId);
            Assert.Equal(1, result.ParentEntityId);
            Assert.Equal(JsonParameter, result.JsonParameters);
            Assert.Collection(result.BatchUploadItems,
                item =>
                {
                    Assert.Collection(item.BatchUploadFields,
                        field =>
                        {
                            Assert.Equal(0, field.Order);
                            Assert.Equal("dataA", field.OriginValue);
                            Assert.Equal("dataA", field.Value);
                        },
                        field =>
                        {
                            Assert.Equal(1, field.Order);
                            Assert.Equal("secondDataA", field.OriginValue);
                            Assert.Equal("secondDataA", field.Value);
                        });
                },
                item =>
                {
                    Assert.Collection(item.BatchUploadFields,
                        field =>
                        {
                            Assert.Equal(0, field.Order);
                            Assert.Equal("dataB", field.OriginValue);
                            Assert.Equal("dataB", field.Value);
                        },
                        field =>
                        {
                            Assert.Equal(1, field.Order);
                            Assert.Equal("secondDataB", field.OriginValue);
                            Assert.Equal("secondDataB", field.Value);
                        });
                });
        }

        #endregion

        #region CreateBatchUpload<T>() tests

        [Fact]
        public void CreateBatchUpload_TParameter_IsConvertedToJsonParameter()
        {
            // arrange
            var data = CreateData();
            var form = CreateForm();
            var parameter = new TParameterTest
            {
                RegistrationTypeId = RegistrationTypeEnum.NewAdult
            };
            var factory = CreateFactory();

            // act
            var result = factory.CreateBatchUpload(data, form, ParentEntityTypeId, UploadedEntityTypeId, batchUploadValueResolverMock.Object, parameter);

            // assert
            Assert.Equal("{\"RegistrationTypeId\":1}", result.JsonParameters);
        }

        #endregion

        #region private methods

        private BatchUploadFactory CreateFactory()
        {
            return new BatchUploadFactory();
        }

        private List<string[]> CreateData()
        {
            return new List<string[]>
            {
                new[] {"dataA", "secondDataA"},
                new[] {"dataB", "secondDataB"}
            };
        }

        private BatchUploadForm CreateForm()
        {
            return new BatchUploadForm
            {
                Name = "Name",
                ParentEntityId = 1,
                BatchUploadTypeId = BatchUploadTypeEnum.StudentRegistrationExcel
            };
        }

        #endregion
    }

    public class TParameterTest
    {
        public RegistrationTypeEnum RegistrationTypeId { get; set; }
    }
}
