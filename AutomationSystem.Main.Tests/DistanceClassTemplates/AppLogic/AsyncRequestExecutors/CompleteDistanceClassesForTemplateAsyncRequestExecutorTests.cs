using System;
using Xunit;
using Moq;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.AsyncRequestExecutors;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.DistanceClassTemplates.System.Models;
using AutomationSystem.Shared.Model;
using Newtonsoft.Json;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic.AsyncRequestExecutors
{
    public class CompleteDistanceClassesForTemplateAsyncRequestExecutorTests
    {
        private readonly Mock<IDistanceClassTemplateService> templateServiceMock;
        private readonly Mock<ITracerFactory> tracerFactoryMock;

        private readonly AsyncRequest request;

        public CompleteDistanceClassesForTemplateAsyncRequestExecutorTests()
        {
            templateServiceMock = new Mock<IDistanceClassTemplateService>();
            tracerFactoryMock = new Mock<ITracerFactory>();
            tracerFactoryMock.Setup(e => e.CreateTracer<CompleteDistanceClassesForTemplateAsyncRequestExecutor>(It.IsAny<long>())).Returns(Mock.Of<ITracer>());

            request = new AsyncRequest
            {
                AsyncRequestTypeId = AsyncRequestTypeEnum.CompleteDistanceClassesForTemplate,
                EntityTypeId = EntityTypeEnum.MainDistanceClassTemplate,
                EntityId = 1,
                JsonParameter = "{ CertificateRootPath: 'path' }"
            };
        }

        #region Execute() tests
        [Theory]
        [InlineData(EntityTypeEnum.MainDistanceClassTemplate, null)]
        [InlineData(EntityTypeEnum.MainDistanceProfile, 1)]
        [InlineData(EntityTypeEnum.MainFormerClass, null)]
        public void Execute_UnsupportedEntityTypeOrEntityIsNull_ThrowsArgumentException(EntityTypeEnum type, long? entityId)
        {
            // arrange
            request.EntityTypeId = type;
            request.EntityId = entityId;
            var executor = CreateExecutor();

            // act & assert
            Assert.Throws<ArgumentException>(() => executor.Execute(request));
        }

        [Fact]
        public void Execute_NullParameters_ThrowsArgumentException()
        {
            // arrange
            request.JsonParameter = null;
            var executor = CreateExecutor();

            // act & assert
            Assert.Throws<ArgumentException>(() => executor.Execute(request));
        }

        [Fact]
        public void Execute_InvalidParameters_ThrowsJsonReaderException()
        {
            // arrange
            request.JsonParameter = "?invalidValue";
            var executor = CreateExecutor();

            // act & assert
            Assert.Throws<JsonReaderException>(() => executor.Execute(request));
        }

        [Fact]
        public void Execute_CompletionSuccess_ReturnsSuccessResult()
        {
            // arrange
            templateServiceMock.Setup(e => e.CompleteDistanceClassTemplate(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(new DistanceClassTemplateCompletionResult(1) { IsSuccess = true });
            var executor = CreateExecutor();

            // act
            var result = executor.Execute(request);
            
            // assert
            Assert.True(result.IsSuccess);
            templateServiceMock.Verify(e => e.CompleteDistanceClassTemplate(request.EntityId.Value, "path"), Times.Once);
        }

        [Fact]
        public void Execute_CompletionFails_ReturnsFailResult()
        {
            // arrange
            var exception = new Exception();
            templateServiceMock.Setup(e => e.CompleteDistanceClassTemplate(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(new DistanceClassTemplateCompletionResult(1) { Exception = exception });
            var executor = CreateExecutor();

            // act
            var result = executor.Execute(request);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(IncidentTypeEnum.AsyncRequestError, result.IncidentTypeId);
            Assert.Equal(EntityTypeEnum.MainDistanceClassTemplate, result.IncidentEntityTypeId);
            Assert.Equal(1, result.IncidentEntityId);
            Assert.Same(exception, result.IncidentException);
        }

        [Fact]
        public void Execute_TemplateServiceReturnsError_ReturnsFailResult()
        {
            // arrange
            templateServiceMock.Setup(e => e.CompleteDistanceClassTemplate(It.IsAny<long>(), It.IsAny<string>())).Throws(new Exception());
            var executor = CreateExecutor();

            // act
            var result = executor.Execute(request);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(IncidentTypeEnum.AsyncRequestError, result.IncidentTypeId);
            Assert.Equal(EntityTypeEnum.MainDistanceClassTemplate, result.IncidentEntityTypeId);
            Assert.Equal(1, result.IncidentEntityId);
        }
        #endregion

        #region private methods
        private CompleteDistanceClassesForTemplateAsyncRequestExecutor CreateExecutor()
        {
            return new CompleteDistanceClassesForTemplateAsyncRequestExecutor(templateServiceMock.Object, tracerFactoryMock.Object);
        }
        #endregion
    }
}
