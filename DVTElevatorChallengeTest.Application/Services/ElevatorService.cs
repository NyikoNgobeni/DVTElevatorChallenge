﻿using System.Threading.Tasks;
using DVTElevatorChallengeTest.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DVTElevatorChallengeTest.Application.Services
{
    /// <summary>
    /// Service to manage elevator operations within a building.
    /// </summary>
    public class ElevatorService : IElevatorService
    {
        private readonly IElevatorDispatcher _dispatcher;
        private readonly ILogger<ElevatorService> _logger;
        private readonly IElevatorRepository _elevatorRepository;

        /// <summary>
        /// Initializes a new instance of the ElevatorService class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher responsible for managing elevator requests.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        /// <param name="elevatorRepository">The repository for managing elevator data.</param>
        public ElevatorService(IElevatorDispatcher dispatcher, ILogger<ElevatorService> logger, IElevatorRepository elevatorRepository)
        {
            _dispatcher = dispatcher;
            _logger = logger;
            _elevatorRepository = elevatorRepository;
        }

        /// <summary>
        /// Calls an elevator to the specified floor with the given number of passengers.
        /// </summary>
        /// <param name="floor">The floor where the elevator is requested.</param>
        /// <param name="passengers">The number of passengers waiting for the elevator.</param>
        public async Task CallElevatorAsync(int floor, int passengers)
        {
            try
            {
                if (floor > 10)
                {
                    _logger.LogError("Floor {Floor} is greater than 10. Cannot call elevator to this floor.", floor);
                    return;
                }

                var elevator = await _dispatcher.DispatchAsync(floor, passengers);
                if (elevator != null)
                {
                    await _elevatorRepository.MoveToFloorAsync(floor);
                    if (await _elevatorRepository.AddPassengersAsync(passengers))
                    {
                        _logger.LogInformation("Elevator {ElevatorId} is moving to floor {Floor}.", elevator.Id, floor);
                    }
                    else
                    {
                        _logger.LogWarning("Elevator is full. Waiting for the next one.");
                    }
                }
                else
                {
                    _logger.LogWarning("No available elevators at the moment. Please wait.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling the elevator.");
            }
        }

        /// <summary>
        /// Moves the elevator to the user's floor and then to the destination floor.
        /// </summary>
        /// <param name="userFloor">The floor where the user is located.</param>
        /// <param name="destinationFloor">The floor where the user wants to go.</param>
        /// <param name="passengers">The number of passengers.</param>
        public async Task MoveElevatorToUserAndDestinationAsync(int userFloor, int destinationFloor, int passengers)
        {
            try
            {
                await _elevatorRepository.MoveElevatorToUserAndDestinationAsync(userFloor, destinationFloor);
                _logger.LogInformation("Elevator moved from floor {UserFloor} to floor {DestinationFloor} with {Passengers} passengers.", userFloor, destinationFloor, passengers);
                DisplayArrivalMessage(destinationFloor);
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
            Console.WriteLine($"The elevator has arrived at floor {floor}. Please exit the elevator.");
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
