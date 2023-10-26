using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Core.BatchUploads.AppLogic.MappingProfiles;
using AutomationSystem.Shared.Model;
using System;
using Xunit;

namespace AutomationSystem.Shared.Tests.BatchUploads.AppLogic.MappingProfiles
{
    public class BatchUploadProfileTests
    {
        #region CreateMap<BatchUpload, BatchUploadListItem>() tests

        [Fact]
        public void Map_BatchUploadTypeIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var batchUpload = new BatchUpload
            {
                BatchUploadType = null
            };
            var mapper = CreateMapper();
            
            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<BatchUploadListItem>(batchUpload));
        }

        [Fact]
        public void Map_BatchUploadStateIsNull_ThrowsInvalidOperationException()
        {
            // arrange
            var batchUpload = new BatchUpload
            {
                BatchUploadType = new BatchUploadType(),
                BatchUploadState = null
            };
            var mapper = CreateMapper();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => mapper.Map<BatchUploadListItem>(batchUpload));
        }

        [Fact]
        public void Map_BatchUpload_IsMappedToBatchUploadListItem()
        {
            // arrange
            var batchUpload = new BatchUpload
            {
                BatchUploadId = 1,
                Name = "Name",
                ParentEntityId = 20,
                BatchUploadTypeId = BatchUploadTypeEnum.StudentRegistrationExcel,
                BatchUploadType = new BatchUploadType
                {
                    Description = "BatchUploadType"
                },
                BatchUploadStateId = BatchUploadStateEnum.InUploading,
                BatchUploadState = new BatchUploadState
                {
                    Description = "BatchUploadState"
                },
                Uploaded = new DateTime(2021, 1, 1),
                IsProcessed = true,
                Processed = new DateTime(2021, 2, 2),
                FileId = 100
            };
            var mapper = CreateMapper();

            // act
            var result = mapper.Map<BatchUploadListItem>(batchUpload);

            // assert
            Assert.Equal(1, result.BatchUploadId);
            Assert.Equal("Name", result.Name);
            Assert.Equal(20, result.ParentEntityId);
            Assert.Equal(BatchUploadTypeEnum.StudentRegistrationExcel, result.BatchUploadTypeId);
            Assert.Equal("BatchUploadType", result.BatchUploadType);
            Assert.Equal(BatchUploadStateEnum.InUploading, result.BatchUploadStateId);
            Assert.Equal("BatchUploadState", result.BatchUploadState);
            Assert.Equal(new DateTime(2021, 1, 1), result.Uploaded);
            Assert.True(result.IsProcessed);
            Assert.Equal(new DateTime(2021, 2, 2), result.Processed);
            Assert.Equal(100, result.FileId);
        }
        #endregion


        #region private methods

        private Mapper CreateMapper()
        {
            var mapCfg = new MapperConfiguration(cfg => cfg.AddProfile(new BatchUploadProfile()));
            return new Mapper(mapCfg);
        }

        #endregion
    }
}
