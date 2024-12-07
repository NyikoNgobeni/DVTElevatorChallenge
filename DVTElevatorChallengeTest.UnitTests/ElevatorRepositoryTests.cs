using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Core.Models;
using DVTElevatorChallengeTest.Infrastructure.Repositories;
using Moq;

namespace DVTElevatorChallengeTest.UnitTests
{
    public class ElevatorRepositoryTests
    {
        private readonly Mock<IElevatorRepository> _mockElevatorRepository;
        private readonly ElevatorRepository _elevatorRepository;

        public ElevatorRepositoryTests()
        {
            var elevator = new Elevator(1) { PassengerCount = 0, CurrentFloor = 1 };
            _mockElevatorRepository = new Mock<IElevatorRepository>();
            _elevatorRepository = new ElevatorRepository(elevator);
        }

        [Test]
        public async Task GetElevatorsAsync_ShouldReturnElevators()
        {
            // Arrange
            int elevatorCount = 5;

            // Act
            var result = await _elevatorRepository.GetElevatorsAsync(elevatorCount);

            // Assert
            Assert.That(result, Has.Count.EqualTo(elevatorCount));
        }

        [Test]
        public async Task AddPassengersAsync_ShouldAddPassengers()
        {
            // Arrange
            int passengers = 5;

            // Act
            var result = await _elevatorRepository.AddPassengersAsync(passengers);

            // Assert
            Assert.That(result);
            var elevator = (Elevator)typeof(ElevatorRepository)
                .GetField("_elevators", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_elevatorRepository);
            Assert.That(elevator.PassengerCount, Is.EqualTo(5));
        }

        [Test]
        public async Task MoveToFloorAsync_ShouldMoveToCorrectFloor()
        {
            // Arrange
            int floor = 5;

            // Act
            await _elevatorRepository.MoveToFloorAsync(floor);

            // Assert
            var elevator = (Elevator)typeof(ElevatorRepository)
                .GetField("_elevators", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_elevatorRepository);
            Assert.That(elevator.CurrentFloor, Is.EqualTo(floor));
        }

        [Test]
        public async Task MoveElevatorToUserDestinationAsync_ShouldMoveToUserAndDestinationFloor()
        {
            // Arrange
            int destinationFloor = 7;

            // Act
            await _elevatorRepository.MoveElevatorToUserDestinationAsync(destinationFloor);

            // Assert
            var elevator = (Elevator)typeof(ElevatorRepository)
                .GetField("_elevators", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_elevatorRepository);
            Assert.That(elevator.CurrentFloor, Is.EqualTo(destinationFloor));
        }

        [Test]
        public void AddPassengersAsync_ShouldThrowArgumentOutOfRangeException_WhenPassengersAreNegative()
        {
            // Arrange
            int passengers = -1;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _elevatorRepository.AddPassengersAsync(passengers));
        }

        [Test]
        public void MoveToFloorAsync_ShouldThrowArgumentOutOfRangeException_WhenFloorIsNegative()
        {
            // Arrange
            int floor = -1;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _elevatorRepository.MoveToFloorAsync(floor));
        }
    }
}
