using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Application.Services;
using DVTElevatorChallengeTest.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;


namespace DVTElevatorChallengeTest.UnitTests
{
    public class ElevatorServiceTests
    {
        private Mock<IElevatorDispatcher> _mockDispatcher;
        private Mock<ILogger<ElevatorService>> _mockLogger;
        private Mock<IElevatorRepository> _mockRepository;
        private ElevatorService _elevatorService;

        [SetUp]
        public void Setup()
        {
            _mockDispatcher = new Mock<IElevatorDispatcher>();
            _mockLogger = new Mock<ILogger<ElevatorService>>();
            _mockRepository = new Mock<IElevatorRepository>();
            _elevatorService = new ElevatorService(_mockDispatcher.Object, _mockLogger.Object, _mockRepository.Object);
        }

        [Test]
        public async Task CallElevatorAsync_FloorGreaterThan10_LogsError()
        {
            // Arrange
            int floor = 11;
            int passengers = 5;
            string errorMessage = $"Floor number {floor} is out of range.";

            // Act
            await _elevatorService.CallElevatorAsync(floor, passengers);

            // Assert
            _mockLogger.Verify(logger =>
                logger.Log(
                    LogLevel.Error,                    
                    It.IsAny<EventId>(),               
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString().Contains(errorMessage)), 
                    null,                              
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
                Times.Once);
        }
        [Test]
        public async Task CallElevatorAsync_ElevatorDispatchedSuccessfully_LogsInformation()
        {
            // Arrange
            int floor = 5;
            int passengers = 3;
            var elevator = new Elevator(1) { CurrentFloor = 1, Direction = ElevatorDirection.Up, PassengerCount = 0 };
            _mockDispatcher.Setup(d => d.DispatchAsync(floor, passengers)).ReturnsAsync(elevator);
            _mockRepository.Setup(r => r.MoveToFloorAsync(floor)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.AddPassengersAsync(passengers)).ReturnsAsync(true);

            // Act
            await _elevatorService.CallElevatorAsync(floor, passengers);

            // Assert
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Elevator {elevator.Id} dispatched to floor {floor}")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }


        [Test]
        public async Task MoveElevatorToUserAndDestinationAsync_LogsInformation()
        {
            // Arrange
            int userFloor = 2;
            int destinationFloor = 8;
            int passengers = 4;
            _mockRepository.Setup(r => r.MoveElevatorToUserAndDestinationAsync(userFloor, destinationFloor)).Returns(Task.CompletedTask);

            // Act
            await _elevatorService.MoveElevatorToUserAndDestinationAsync(userFloor, destinationFloor, passengers);

            // Assert
            _mockLogger.Verify(logger => logger.LogInformation(It.IsAny<string>(), userFloor, destinationFloor, passengers), Times.Once);
        }

        [Test]
        public async Task DisplayStatusAsync_LogsElevatorStatus()
        {
            // Arrange
            var elevators = new List<Elevator>
            {
                new(1) { CurrentFloor = 1, Direction = ElevatorDirection.Up, PassengerCount = 2 },
                new(2) { CurrentFloor = 3, Direction = ElevatorDirection.Down, PassengerCount = 1 }
            };
            _mockRepository.Setup(r => r.GetElevatorsAsync(5)).ReturnsAsync(elevators);

            // Act
            await _elevatorService.DisplayStatusAsync();

            // Assert
            _mockLogger.Verify(static logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(elevators.Count));
        }

        [Test]
        public async Task CallElevatorAsync_FloorLessThan1_LogsError()
        {
            // Arrange
            int floor = 0;
            int passengers = 5;
            string errorMessage = "Floor number {Floor} is out of range.";

            // Act
            await _elevatorService.CallElevatorAsync(floor, passengers);

            // Assert
            _mockLogger.Verify(logger => logger.LogError(errorMessage, floor), Times.Once);
        }

        [Test]
        public async Task CallElevatorAsync_PassengersExceedMax_LogsError()
        {
            // Arrange
            int floor = 5;
            int passengers = 15;
            string errorMessage = "Number of passengers {Passengers} exceeds the maximum limit.";

            // Act
            await _elevatorService.CallElevatorAsync(floor, passengers);

            // Assert
            _mockLogger.Verify(logger => logger.LogError(errorMessage, passengers), Times.Once);
        }

        [Test]
        public async Task MoveElevatorToUserAndDestinationAsync_InvalidDestinationFloor_LogsError()
        {
            // Arrange
            int userFloor = 2;
            int destinationFloor = 11;
            int passengers = 4;
            string errorMessage = "Destination floor {DestinationFloor} is out of range.";

            // Act
            await _elevatorService.MoveElevatorToUserAndDestinationAsync(userFloor, destinationFloor, passengers);

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
