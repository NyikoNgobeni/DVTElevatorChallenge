using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Core.Models;

namespace DVTElevatorChallengeTest.Infrastructure.Repositories
{
    public class ElevatorRepository(Elevator elevators) : IElevatorRepository
    {
        private readonly Elevator _elevators = elevators;


        /// <summary>
        /// Asynchronously retrieves the list of elevators.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of elevators.</returns>
        public Task<List<Elevator>> GetElevatorsAsync(int elevatorCount) 
        { 
            return Task.FromResult(Enumerable.Range(1, 
                elevatorCount).Select(id => new Elevator(id)).ToList()); 
        }

        /// <summary>
        /// Asynchronously attempts to add passengers to the elevator.
        /// </summary>
        /// <param name="passengers">The number of passengers to add.</param>
        /// <returns>A task with a result indicating whether the passengers were added successfully.</returns>
        public async Task<bool> AddPassengersAsync(int passengers)
        {
            if (passengers <= 0)
                throw new ArgumentOutOfRangeException(nameof(passengers), "Number of passengers must be greater than zero.");

            // Simulate delay for adding passengers 
            await Task.Delay(100); // Simulating some asynchronous work

            if (_elevators.PassengerCount + passengers > _elevators.MaxPassengers)
                return false;

            _elevators.PassengerCount += passengers;
            return true;
        }

        /// <summary>
        /// Moves the elevator to the specified floor asynchronously.
        /// </summary>
        /// <param name="floor">Target floor.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
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
                // Simulate movement with cancellation support
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
    }
}