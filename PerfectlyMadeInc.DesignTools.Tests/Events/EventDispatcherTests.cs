using System;
using System.Collections.Generic;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;
using PerfectlyMadeInc.DesignTools.Events;
using PerfectlyMadeInc.DesignTools.Tests.TestingTools;
using Xunit;
using Xunit.Abstractions;

namespace PerfectlyMadeInc.DesignTools.Tests.Events
{
    public class EventDispatcherTests : TestsWithTrace
    {
        private readonly Mock<IServiceProvider> serviceProviderMock;

        private readonly Mock<IEventHandler<TestEventOne>> eventHandlerFirstOneMock;
        private readonly Mock<IEventHandler<TestEventOne>> eventHandlerSecondOneMock;
        private readonly Mock<IEventHandler<TestEventTwo>> eventHandlerTwoMock;

        private readonly Mock<IEventChecker<TestEventOne>> eventCheckerFirstOneMock;
        private readonly Mock<IEventChecker<TestEventOne>> eventCheckerSecondOneMock;

        public EventDispatcherTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            eventHandlerFirstOneMock = new Mock<IEventHandler<TestEventOne>>();
            eventHandlerFirstOneMock.Setup(e => e.HandleEvent(It.IsAny<TestEventOne>())).Returns(Result.Success("First one success"));
            eventHandlerSecondOneMock = new Mock<IEventHandler<TestEventOne>>();
            eventHandlerSecondOneMock.Setup(e => e.HandleEvent(It.IsAny<TestEventOne>())).Returns(Result.Skipped("Second one skipped"));
            eventHandlerTwoMock = new Mock<IEventHandler<TestEventTwo>>();
            eventHandlerTwoMock.Setup(e => e.HandleEvent(It.IsAny<TestEventTwo>())).Returns(Result.Success("Two skipped"));

            eventCheckerFirstOneMock = new Mock<IEventChecker<TestEventOne>>();
            eventCheckerFirstOneMock.Setup(e => e.CheckEvent(It.IsAny<TestEventOne>())).Returns(true);
            eventCheckerSecondOneMock = new Mock<IEventChecker<TestEventOne>>();
            eventCheckerSecondOneMock.Setup(e => e.CheckEvent(It.IsAny<TestEventOne>())).Returns(true);

            serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(e => e.GetService(typeof(IEnumerable<IEventHandler<TestEventOne>>)))
                .Returns(new List<IEventHandler<TestEventOne>> { eventHandlerFirstOneMock.Object, eventHandlerSecondOneMock.Object });
            serviceProviderMock.Setup(e => e.GetService(typeof(IEnumerable<IEventHandler<TestEventTwo>>)))
                .Returns(new List<IEventHandler<TestEventTwo>> { eventHandlerTwoMock.Object });
            serviceProviderMock.Setup(e => e.GetService(typeof(IEnumerable<IEventChecker<TestEventOne>>)))
                .Returns(new List<IEventChecker<TestEventOne>> { eventCheckerFirstOneMock.Object, eventCheckerSecondOneMock.Object });
            serviceProviderMock.Setup(e => e.GetService(typeof(IEnumerable<IEventChecker<TestEventTwo>>)))
                .Returns(new List<IEventChecker<TestEventTwo>>());
        }

        #region Dispatch<>() tests

        [Fact]
        public void Dispatch_TestEventOneDispatched_BothEventHandlersCalled()
        {
            // arrange
            var evnt = new TestEventOne();
            var dispatcher = CreateDispatcher();

            // act
            dispatcher.Dispatch(evnt);

            // assert
            eventHandlerFirstOneMock.Verify(e => e.HandleEvent(evnt), Times.Once);
            eventHandlerSecondOneMock.Verify(e => e.HandleEvent(evnt), Times.Once);
            eventHandlerTwoMock.Verify(e => e.HandleEvent(It.IsAny<TestEventTwo>()), Times.Never);
        }

        [Fact]
        public void Dispatch_TestEventTwoDispatchedTwice_EventHandlerCalledTwice()
        {
            // arrange
            var dispatcher = CreateDispatcher();

            // act
            dispatcher.Dispatch(new TestEventTwo());
            dispatcher.Dispatch(new TestEventTwo());

            // assert
            eventHandlerTwoMock.Verify(e => e.HandleEvent(It.IsAny<TestEventTwo>()), Times.Exactly(2));
        }

        [Fact]
        public void Dispatch_HandlerCausesArgumentException_ArgumentExceptionIsThrown()
        {
            // arrange
            eventHandlerTwoMock.Setup(e => e.HandleEvent(It.IsAny<TestEventTwo>())).Throws<ArgumentException>();
            var dispatcher = CreateDispatcher();

            // act & assert
            Assert.Throws<ArgumentException>(() => dispatcher.Dispatch(new TestEventTwo()));
        }

        [Fact]
        public void Dispatch_HandlerReturnsErrorWithInvalidOperationException_InvalidOperationExceptionIsThrown()
        {
            // arrange
            eventHandlerTwoMock.Setup(e => e.HandleEvent(It.IsAny<TestEventTwo>())).Returns(Result.Error(new InvalidOperationException()));
            var dispatcher = CreateDispatcher();

            // act & assert
            Assert.Throws<InvalidOperationException>(() => dispatcher.Dispatch(new TestEventTwo()));
        }

        [Fact]
        public void Dispatch_HandlerReturnsErrorWithoutException_ExceptionIsThrown()
        {
            // arrange
            eventHandlerTwoMock.Setup(e => e.HandleEvent(It.IsAny<TestEventTwo>())).Returns(Result.Error(null, "Something goes wrong"));
            var dispatcher = CreateDispatcher();

            // act & assert
            Assert.Throws<Exception>(() => dispatcher.Dispatch(new TestEventTwo()));
        }

        [Fact]
        public void Dispatch_FirstHandlerCausesError_SecondHandlerIsNotCalledCalled()
        {
            // arrange
            eventHandlerFirstOneMock.Setup(e => e.HandleEvent(It.IsAny<TestEventOne>())).Throws<ArgumentException>();
            var dispatcher = CreateDispatcher();

            // act
            Assert.Throws<ArgumentException>(() => dispatcher.Dispatch(new TestEventOne()));

            // assert
            eventHandlerFirstOneMock.Verify(e => e.HandleEvent(It.IsAny<TestEventOne>()), Times.Once);
            eventHandlerSecondOneMock.Verify(e => e.HandleEvent(It.IsAny<TestEventOne>()), Times.Never);
        }

        #endregion

        #region Check<>() tests

        [Fact]
        public void Check_TestEventOneAllSuccess_ReturnsTrue()
        {
            // arrange
            var evnt = new TestEventOne();
            var dispatcher = CreateDispatcher();

            // act
            var result = dispatcher.Check(evnt);

            // assert
            Assert.True(result);
            eventCheckerFirstOneMock.Verify(e => e.CheckEvent(evnt), Times.Once);
            eventCheckerSecondOneMock.Verify(e => e.CheckEvent(evnt), Times.Once);
        }

        [Fact]
        public void Check_TestEventOneSecondCheckersReturnFalse_ReturnsFalse()
        {
            // arrange
            var evnt = new TestEventOne();
            eventCheckerSecondOneMock.Setup(e => e.CheckEvent(It.IsAny<TestEventOne>())).Returns(false);
            var dispatcher = CreateDispatcher();

            // act
            var result = dispatcher.Check(evnt);

            // assert
            Assert.False(result);
            eventCheckerFirstOneMock.Verify(e => e.CheckEvent(evnt), Times.Once);
            eventCheckerSecondOneMock.Verify(e => e.CheckEvent(evnt), Times.Once);
        }

        [Fact]
        public void Check_TestEventOneFirstCheckersReturnFalse_ReturnsFalseSecondNotCalled()
        {
            // arrange
            var evnt = new TestEventOne();
            eventCheckerFirstOneMock.Setup(e => e.CheckEvent(It.IsAny<TestEventOne>())).Returns(false);
            var dispatcher = CreateDispatcher();

            // act
            var result = dispatcher.Check(evnt);

            // assert
            Assert.False(result);
            eventCheckerFirstOneMock.Verify(e => e.CheckEvent(evnt), Times.Once);
            eventCheckerSecondOneMock.Verify(e => e.CheckEvent(It.IsAny<TestEventOne>()), Times.Never);
        }

        [Fact]
        public void Check_TestEventTwo_ReturnsTrueCheckersOneNotCalled()
        {
            // arrange
            var evnt = new TestEventTwo();
            var dispatcher = CreateDispatcher();

            // act
            var result = dispatcher.Check(evnt);

            // assert
            Assert.True(result);
            eventCheckerFirstOneMock.Verify(e => e.CheckEvent(It.IsAny<TestEventOne>()), Times.Never);
            eventCheckerSecondOneMock.Verify(e => e.CheckEvent(It.IsAny<TestEventOne>()), Times.Never);
        }

        #endregion 

        #region private methods

        private EventDispatcher CreateDispatcher()
        {
            return new EventDispatcher(
                tracerFactory,
                serviceProviderMock.Object);
        }

        #endregion

        #region test events

        public class TestEventOne : BaseEvent
        {
            public override string ToString()
            {
                return "TestEventOne";
            }
        }

        public class TestEventTwo : BaseEvent
        {
            public override string ToString()
            {
                return "TestEventTwo";
            }
        }

        #endregion
    }
}
