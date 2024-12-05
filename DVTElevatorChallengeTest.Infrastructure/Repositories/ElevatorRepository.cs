using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Core.Models;

namespace DVTElevatorChallengeTest.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing elevator operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ElevatorRepository"/> class.
    /// </remarks>
    /// <param name="elevators">The elevator instance.</param>
    public class ElevatorRepository(Elevator elevators) : IElevatorRepository
    {
        private readonly Elevator _elevators = elevators;

        /// <summary>
        /// Gets a list of elevators with random current floors.
        /// </summary>
        /// <param name="elevatorCount">The number of elevators to generate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of elevators.</returns>
        public Task<List<Elevator>> GetElevatorsAsync(int elevatorCount)
        {
            var random = new Random();
            var elevators = Enumerable.Range(1, elevatorCount)
                                      .Select(id => new Elevator(id) { CurrentFloor = random.Next(1, 11) })
                                      .ToList();
            return Task.FromResult(elevators);
        }

        /// <summary>
        /// Adds passengers to the elevator.
        /// </summary>
        /// <param name="passengers">The number of passengers to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the passengers were added successfully.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of passengers is less than or equal to zero.</exception>
        public async Task<bool> AddPassengersAsync(int passengers)
        {
            if (passengers <= 0)
                throw new ArgumentOutOfRangeException(nameof(passengers), "Number of passengers must be greater than zero.");

            await Task.Delay(100); // Simulating some asynchronous work

            if (_elevators.PassengerCount + passengers > _elevators.MaxPassengers)
                return false;

            _elevators.PassengerCount += passengers;
            return true;
        }

        /// <summary>
        /// Moves the elevator to the specified floor.
        /// </summary>
        /// <param name="floor">The floor to move to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the floor number is negative.</exception>
        public async Task MoveToFloorAsync(int floor)
        {
            if (floor < 0)
                throw new ArgumentOutOfRangeException(nameof(floor), "Floor number cannot be negative.");

            if (floor == _elevators.CurrentFloor)
                return;

            _elevators.State = ElevatorState.Moving;
            _elevators.Direction = floor > _elevators.CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            try
            {
                var travelTime = Math.Abs(floor - _elevators.CurrentFloor) * 1000;
                await Task.Delay(travelTime);

                _elevators.CurrentFloor = floor;
            }
            finally
            {
                _elevators.State = ElevatorState.Idle;
                _elevators.Direction = ElevatorDirection.Stationary;
            }
        }

        /// <summary>
        /// Moves the elevator to the user's floor and then to the destination floor.
        /// </summary>
        /// <param name="userFloor">The floor where the user is located.</param>
        /// <param name="destinationFloor">The floor where the user wants to go.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task MoveElevatorToUserAndDestinationAsync(int userFloor, int destinationFloor)
        {
            // Move elevator to user's floor
            await MoveToFloorAsync(userFloor);

            await AddPassengersAsync(1);

            // Move elevator to destination floor
            await MoveToFloorAsync(destinationFloor);
        }
    }
}
