using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Core.BatchUploads.System;
using AutomationSystem.Shared.Model;
using Moq;
using Xunit;

namespace AutomationSystem.Shared.Tests.BatchUploads.System
{
    public class BatchUploadServiceTests
    {
        private readonly Mock<IBatchUploadDatabaseLayer> batchUploadDbMock;
        private readonly Mock<ICoreMapper> coreMapperMock;

        public BatchUploadServiceTests()
        {
            batchUploadDbMock = new Mock<IBatchUploadDatabaseLayer>();
            coreMapperMock = new Mock<ICoreMapper>();
        }

        #region GetBatchUploadListItems() tests

        [Fact]
        public void GetBatchUploadListItems_ExpectedIncludesPassToGetBatchUploadByParent()
        {
            // arrange
            batchUploadDbMock.Setup(e => e.GetBatchUploadByParent(It.IsAny<EntityTypeEnum?>(), It.IsAny<long?>(), It.IsAny<BatchUploadIncludes>())).Returns(new List<BatchUpload>());
            var service = CreateService();

            // act
            service.GetBatchUploadListItems(EntityTypeEnum.MainClass, 1);

            // assert
            batchUploadDbMock.Verify(e => e.GetBatchUploadByParent(EntityTypeEnum.MainClass, 1, BatchUploadIncludes.BatchUploadState | BatchUploadIncludes.BatchUploadType), Times.Once);
        }

        [Fact]
        public void GetBatchUploadListItems_BatchUploads_ReturnsBatchUploadListItems()
        {
            // arrange
            var listItem = new BatchUploadListItem();
            batchUploadDbMock.Setup(e => e.GetBatchUploadByParent(It.IsAny<EntityTypeEnum?>(), It.IsAny<long?>(), It.IsAny<BatchUploadIncludes>())).Returns(CreateBatchUploads);
            coreMapperMock.Setup(e => e.Map<BatchUploadListItem>(It.IsAny<BatchUpload>())).Returns(listItem);
            var service = CreateService();

            // act
            var result = service.GetBatchUploadListItems(EntityTypeEnum.MainClass, 1);

            // assert
            Assert.Collection(result,
                item => Assert.Same(listItem, item));
            coreMapperMock.Verify(e => e.Map<BatchUploadListItem>(It.Is<BatchUpload>(x => x.BatchUploadId == 4)), Times.Once);
        }

        #endregion

        #region private methods

        private BatchUploadService CreateService()
        {
            return new BatchUploadService(batchUploadDbMock.Object, coreMapperMock.Object);
        }

        private List<BatchUpload> CreateBatchUploads()
        {
            return new List<BatchUpload>
            {
                new BatchUpload
                {
                    BatchUploadId = 4
                }
            };
        }

        #endregion
    }
}
