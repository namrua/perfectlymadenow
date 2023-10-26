using System;
using AutomationSystem.Shared.Core.Maintenance.Data;
using AutomationSystem.Shared.Core.Maintenance.System;
using Moq;
using Xunit;

namespace AutomationSystem.Shared.Tests.Maintenance.System
{
    public class CoreMaintenanceServiceTests
    {
        private readonly Mock<IMaintenanceDatabaseLayer> maintenanceDbMock;

        public CoreMaintenanceServiceTests()
        {
            maintenanceDbMock = new Mock<IMaintenanceDatabaseLayer>();
        }

        #region ClearDatabaseFiles() tests

        [Fact]
        public void ClearDatabaseFiles_ClearFilesCalledOnDb_ReturnsAffectedRows()
        {
            // arrange
            maintenanceDbMock.Setup(e => e.ClearFiles(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(123);
            var toDate = DateTime.Now;
            var service = CreateService();

            // act
            var result = service.ClearDatabaseFiles(toDate, 10);

            // assert
            Assert.Equal(123, result);
            maintenanceDbMock.Verify(e => e.ClearFiles(toDate, 10), Times.Once);
        }

        #endregion

        #region private methods

        private CoreMaintenanceService CreateService()
        {
            return new CoreMaintenanceService(maintenanceDbMock.Object);
        }

        #endregion
    }
}
