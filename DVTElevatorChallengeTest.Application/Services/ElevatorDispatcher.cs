using DVTElevatorChallengeTest.Application.Interfaces;
using DVTElevatorChallengeTest.Core.Models;

namespace DVTElevatorChallengeTest.Application.Services
{
    /// <summary>
    /// The ElevatorDispatcher class is responsible for dispatching the most suitable elevator
    /// based on the requested floor and the number of passengers.
    /// </summary>
    public class ElevatorDispatcher(IElevatorRepository elevatorRepository) : IElevatorDispatcher
    {
        private readonly IElevatorRepository _elevatorRepository = elevatorRepository;

        /// <summary>
        /// Dispatches the most suitable elevator based on the requested floor and the number of passengers.
        /// </summary>
        /// <param name="requestedFloor">The floor where the elevator is requested.</param>
        /// <param name="passengers">The number of passengers requesting the elevator.</param>
        /// <returns>The most suitable elevator or null if no suitable elevator is found.</returns>
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
