using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Core.Models;


namespace DVTElevatorChallengeTest.Application.Services
{
    public class ElevatorDispatcher(IElevatorRepository elevatorRepository) : IElevatorDispatcher
    {
        private readonly IElevatorRepository _elevatorRepository = elevatorRepository;

        public async Task<Elevator> DispatchAsync(int requestedFloor, int passengers)
        {
            var availableElevators = (await _elevatorRepository.GetElevatorsAsync(5))
                .Where(e => !e.IsMoving && e.PassengerCount + passengers <= e.MaxPassengers)
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .ToList();

            return availableElevators.FirstOrDefault();
        }
    }
}

