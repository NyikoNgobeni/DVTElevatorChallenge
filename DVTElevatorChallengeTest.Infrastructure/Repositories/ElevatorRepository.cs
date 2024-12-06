using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Core.Models;

namespace DVTElevatorChallengeTest.Infrastructure.Repositories
{
    public class ElevatorRepository(Elevator elevators) : IElevatorRepository
    {
        private readonly Elevator _elevators = elevators;
        private const int DefaultPassengerCount = 1;

        public Task<List<Elevator>> GetElevatorsAsync(int elevatorCount)
        {
            var random = new Random();
            var elevators = Enumerable.Range(1, elevatorCount)
                                      .Select(id => new Elevator(id) { CurrentFloor = random.Next(1, 11) })
                                      .ToList();
            return Task.FromResult(elevators);
        }

        public async Task<bool> AddPassengersAsync(int passengers)
        {
            await Task.Delay(100); // Simulating some asynchronous work

            if (_elevators.PassengerCount + passengers > _elevators.MaxPassengers)
                return false;

            _elevators.PassengerCount += passengers;
            return true;
        }

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

        public async Task MoveElevatorToUserAndDestinationAsync(int userFloor, int destinationFloor)
        {
            // Move elevator to user's floor
            await MoveToFloorAsync(userFloor);

            await AddPassengersAsync(DefaultPassengerCount);

            // Move elevator to destination floor
            await MoveToFloorAsync(destinationFloor);
        }
    }
}
