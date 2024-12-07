using System.Threading.Tasks;
using DVTElevatorChallengeTest.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DVTElevatorChallengeTest.Application.Services
{
    /// <summary>
    /// Service to manage elevator operations within a building.
    /// </summary>
    public class ElevatorService : IElevatorService
    {
        private readonly ILogger<ElevatorService> _logger;
        private readonly IElevatorRepository _elevatorRepository;

        /// <summary>
        /// Initializes a new instance of the ElevatorService class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher responsible for managing elevator requests.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        /// <param name="elevatorRepository">The repository for managing elevator data.</param>
        public ElevatorService(Logger<ElevatorService> logger, IElevatorRepository elevatorRepository)
        {
            _logger = logger;
            _elevatorRepository = elevatorRepository;
        }


        /// <summary>
        /// Moves the elevator to the user's floor and then to the destination floor.
        /// </summary>
        /// <param name="destinationFloor">The floor where the user wants to go.</param>
        /// <param name="passengers">The number of passengers.</param>
        public async Task MoveElevatorToUserDestinationAsync(int destinationFloor, int passengers)
        {
            try
            {
                await _elevatorRepository.MoveElevatorToUserDestinationAsync(destinationFloor);

                _logger.LogInformation("Elevator is moving to floor {DestinationFloor} with {Passengers} passengers.", destinationFloor, passengers);

                DisplayArrivalMessage(destinationFloor);

                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while moving the elevator.");
            }
        }

        /// <summary>
        /// Displays a message when the elevator reaches the destination floor.
        /// </summary>
        /// <param name="floor">The destination floor.</param>
        private static void DisplayArrivalMessage(int floor)
        {
            Console.WriteLine($"You have arrived at your destination floor {floor}. Please exit the elevator.");
        }

        /// <summary>
        /// Displays the current status of all elevators in the building.
        /// </summary>
        public async Task DisplayStatusAsync()
        {
            try
            {
                var elevators = await _elevatorRepository.GetElevatorsAsync(5);
                foreach (var elevator in elevators)
                {
                    _logger.LogInformation($"Elevator {elevator.Id}: Floor {elevator.CurrentFloor}, " +
                                           $"Direction: {elevator.Direction}, " +
                                           $"Passengers: {elevator.PassengerCount}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while displaying the status.");
            }
        }
    }
}
