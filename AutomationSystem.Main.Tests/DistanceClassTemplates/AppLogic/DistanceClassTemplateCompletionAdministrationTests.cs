using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Completion;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic
{
    public class DistanceClassTemplateCompletionAdministrationTests
    {
        private readonly Mock<IDistanceClassTemplateDatabaseLayer> distanceTemplateDbMock;
        private readonly Mock<IMainMapper> mainMapperMock;
        private readonly Mock<IMainAsyncRequestManager> mainAsyncRequestManagerMock;
        private readonly Mock<IDistanceClassTemplateHelper> templateHelperMock;
        private readonly Mock<IDistanceClassTemplateOperationChecker> templateOperationCheckerMock;

        public DistanceClassTemplateCompletionAdministrationTests()
        {
            distanceTemplateDbMock = new Mock<IDistanceClassTemplateDatabaseLayer>();
            mainMapperMock = new Mock<IMainMapper>();
            mainAsyncRequestManagerMock = new Mock<IMainAsyncRequestManager>();
            templateHelperMock = new Mock<IDistanceClassTemplateHelper>();
            templateOperationCheckerMock = new Mock<IDistanceClassTemplateOperationChecker>();
        }

        #region GetDistanceClassTemplateCompletionPageModel() tests
        [Fact]
        public void GetDistanceClassTemplateCompletionPageModel_IncludesPassToGetDistanceClassTemplateById_ReturnsDistanceClassTemplate()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateCompletionShortDetail>(It.IsAny<DistanceClassTemplate>())).Returns(new DistanceClassTemplateCompletionShortDetail());
            var admin = CreateAdmin();

            // act
            admin.GetDistanceClassTemplateCompletionPageModel(1);

            // assert
            distanceTemplateDbMock.Verify(e => e.GetDistanceClassTemplateById(1, DistanceClassTemplateIncludes.ClassType), Times.Once);
        }

        [Fact]
        public void GetDistanceClassTemplateCompletionPageModel_DistanceClassTemplateIsNull_ThrowsArgumentException()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns((DistanceClassTemplate)null);
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<ArgumentException>(() => admin.GetDistanceClassTemplateCompletionPageModel(1));
        }
        
        [Fact]
        public void GetDistanceClassTemplateCompletionPageModel_DistanceClassTemplateOperationIsAllowed_ReturnsTrue()
        {
            // arrange
            var detail = new DistanceClassTemplateCompletionShortDetail
            {
                TemplateState = DistanceClassTemplateState.Approved
            };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateCompletionShortDetail>(It.IsAny<DistanceClassTemplate>())).Returns(detail);
            templateOperationCheckerMock.Setup(e => e.IsOperationAllowed(It.IsAny<DistanceClassTemplateOperation>(), It.IsAny<DistanceClassTemplateState>())).Returns(true);
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplateCompletionPageModel(15);

            // assert
            Assert.True(result.CanComplete);
            templateOperationCheckerMock.Verify(e => e.IsOperationAllowed(DistanceClassTemplateOperation.Complete, DistanceClassTemplateState.Approved), Times.Once);
        }

        [Fact]
        public void GetDistanceClassTemplateCompletionPageModel_ForDistanceClassTemplateId_AsyncRequestDetailIsLoaded()
        {
            // arrange
            var asyncRequestDetails = new List<AsyncRequestDetail>();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateCompletionShortDetail>(It.IsAny<DistanceClassTemplate>())).Returns(new DistanceClassTemplateCompletionShortDetail());
            mainAsyncRequestManagerMock.Setup(e => e.GetLastRequestsByEntityAndTypes(
                It.IsAny<EntityTypeEnum>(),
                It.IsAny<long>(),
                It.IsAny<AsyncRequestTypeEnum>()))
                .Returns(asyncRequestDetails);
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplateCompletionPageModel(4);

            // assert
            Assert.Same(asyncRequestDetails, result.GeneratingRequests);
            mainAsyncRequestManagerMock.Verify(e => e.GetLastRequestsByEntityAndTypes(
                EntityTypeEnum.MainDistanceClassTemplate,
                4,
                AsyncRequestTypeEnum.CompleteDistanceClassesForTemplate),
                Times.Once);
        }

        [Fact]
        public void GetDistanceClassTemplateCompletionPageModel_ForDistanceClassTemplateIdInDb_ReturnsDistanceClassTemplateCompletionPageModel()
        {
            // arrange
            var template = new DistanceClassTemplate
            {
                Completed = new DateTime(2021, 1, 2),
                AutomationCompleteTime = new DateTime(2021, 1, 1)
            };
            var detail = new DistanceClassTemplateCompletionShortDetail();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateCompletionShortDetail>(It.IsAny<DistanceClassTemplate>())).Returns(detail);
            var admin = CreateAdmin();

            // act
            var result = admin.GetDistanceClassTemplateCompletionPageModel(2);

            // assert
            Assert.Same(detail, result.ShortDetail);
            Assert.Equal(new DateTime(2021, 1, 2), result.Completed);
            Assert.Equal(new DateTime(2021, 1, 1), result.AutomationCompleteTime);
            mainMapperMock.Verify(e => e.Map<DistanceClassTemplateCompletionShortDetail>(template), Times.Once);
        }
        #endregion

        #region GetDistanceClassTemplateCompletionFormById() tests
        [Fact]
        public void GetDistanceClassTemplateCompletionFormById_IncludesPassToGetDistanceClassTemplateById_ReturnsDistanceClassTemplate()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            var admin = CreateAdmin();

            // act
            admin.GetDistanceClassTemplateCompletionFormById(1);

            // assert
            distanceTemplateDbMock.Verify(e => e.GetDistanceClassTemplateById(1, DistanceClassTemplateIncludes.None), Times.Once);
        }

        [Fact]
        public void GetDistanceClassTemplateCompletionFormById_ForDistanceClassTemplateInDb_ReturnsDistanceClassTemplateCompletionForm()
        {
            // arrange
            var template = new DistanceClassTemplate();
            var form = new DistanceClassTemplateCompletionForm();
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            mainMapperMock.Setup(e => e.Map<DistanceClassTemplateCompletionForm>(It.IsAny<DistanceClassTemplate>())).Returns(form);
            var admin = CreateAdmin();

            // act 
            var result = admin.GetDistanceClassTemplateCompletionFormById(4);

            // assert
            Assert.Same(form, result);
            mainMapperMock.Verify(e => e.Map<DistanceClassTemplateCompletionForm>(template), Times.Once);
        }
        #endregion

        #region SaveDistanceClassTemplateCompletionSettings() tests
        [Fact]
        public void SaveDistanceClassTemplateCompletionSettings_DistanceClassTemplateCompletionForm_UpdateDistanceClassTemplateCompletionSettingsIsCalled()
        {
            // arrange
            var dateTime = new DateTime(2021, 1, 1);
            var form = new DistanceClassTemplateCompletionForm
            {
                DistanceClassTemplateId = 1,
                AutomationCompleteTime = dateTime
            };
            distanceTemplateDbMock.Setup(e => e.UpdateDistanceClassTemplateCompletionSettings(It.IsAny<long>(), It.IsAny<DateTime?>()));
            var admin = CreateAdmin();

            // act
            admin.SaveDistanceClassTemplateCompletionSettings(form);

            // assert
            distanceTemplateDbMock.Verify(e => e.UpdateDistanceClassTemplateCompletionSettings(
                It.Is<long>(x => x == 1),
                It.Is<DateTime?>(x => x == dateTime)),
                Times.Once);
        }
        #endregion

        #region CompleteDistanceClassTemplate() tests
        [Fact]
        public void CompleteDistanceClassTemplate_DistanceClassTemplateOperationIsInvalid_ThrowsInvalidOperationException()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            templateOperationCheckerMock.Setup(e => e.CheckOperation(It.IsAny<DistanceClassTemplateOperation>(), It.IsAny<DistanceClassTemplateState>())).Throws(new InvalidOperationException());
            var admin = CreateAdmin();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => admin.CompleteDistanceClassTemplate(1));
        }

        [Fact]
        public void CompleteDistanceClassTemplate_ForDistanceClassTemplateIdInDb_AsyncRequestIsCalled()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            templateHelperMock.Setup(e => e.GetDistanceClassTemplateState(It.IsAny<DistanceClassTemplate>())).Returns(DistanceClassTemplateState.Approved);
            mainAsyncRequestManagerMock.Setup(e => e.AddCompletionDistanceClassForTemplateAsyncRequest(It.IsAny<long>(), It.IsAny<int>())).Returns(new AsyncRequest());
            var admin = CreateAdmin();

            // act
            admin.CompleteDistanceClassTemplate(1);

            // assert
            mainAsyncRequestManagerMock.Verify(e => e.AddCompletionDistanceClassForTemplateAsyncRequest(1, (int)SeverityEnum.High), Times.Once);
        }
        #endregion

        #region private methods
        private DistanceClassTemplateCompletionAdministration CreateAdmin()
        {
            return new DistanceClassTemplateCompletionAdministration(
                distanceTemplateDbMock.Object,
                mainMapperMock.Object,
                mainAsyncRequestManagerMock.Object,
                templateHelperMock.Object,
                templateOperationCheckerMock.Object);
        }
        #endregion
    }
}
