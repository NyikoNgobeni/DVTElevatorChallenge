using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Application.Services;
using DVTElevatorChallengeTest.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;


namespace DVTElevatorChallengeTest.UnitTests
{
    public class ElevatorServiceTests
    {
        private Mock<ILogger<ElevatorService>> _mockLogger;
        private Mock<IElevatorRepository> _mockRepository;
        private ElevatorService _elevatorService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<ElevatorService>>();
            _mockRepository = new Mock<IElevatorRepository>();
            _elevatorService = new ElevatorService((Logger<ElevatorService>)_mockLogger.Object, _mockRepository.Object);
        }

        [Test]
        public async Task MoveElevatorToUserAndDestinationAsync_LogsInformation()
        {
            // Arrange
            int destinationFloor = 8;
            int passengers = 4;
            _mockRepository.Setup(r => r.MoveElevatorToUserDestinationAsync(destinationFloor)).Returns(Task.CompletedTask);

            // Act
            await _elevatorService.MoveElevatorToUserDestinationAsync(destinationFloor, passengers);

            // Assert
            _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), destinationFloor, passengers), Times.Once);
        }

        [Test]
        public async Task DisplayStatusAsync_LogsElevatorStatus()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new() { CurrentFloor = 1, Direction = ElevatorDirection.Up, PassengerCount = 2 },
                new() { CurrentFloor = 3, Direction = ElevatorDirection.Down, PassengerCount = 1 }
            };
            _mockRepository.Setup(r => r.GetElevatorsAsync(5)).ReturnsAsync(elevators);

            // Act
            await _elevatorService.DisplayStatusAsync();

            // Assert
            _mockLogger.Verify(static logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(elevators.Count));
        }

        [Test]
        public async Task MoveElevatorToUserDestinationAsync_InvalidDestinationFloor_LogsError()
        {
            // Arrange
            int destinationFloor = 11;
            int passengers = 4;
            string errorMessage = "Destination floor {DestinationFloor} is out of range.";

            // Act
            await _elevatorService.MoveElevatorToUserDestinationAsync(destinationFloor, passengers);

            // Assert
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(errorMessage)),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

    }
}
