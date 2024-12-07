using System.Threading.Tasks;
using DVTElevatorChallengeTest.Application.Services;
using DVTElevatorChallengeTest.Core.Models;
using DVTElevatorChallengeTest.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace DVTElevatorChallengeTest.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(static builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<ElevatorService>();
            var elevator = new Elevator(1); // Create an instance of Elevator
            var elevatorRepository = new ElevatorRepository(elevator); // Pass the instance to the constructor
            var dispatcher = new ElevatorDispatcher(elevatorRepository);
            var service = new ElevatorService((Logger<ElevatorService>)logger, elevatorRepository);

            Console.WriteLine("Welcome to the DVT Elevator Simulation!");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Elevator Status ===");
                await service.DisplayStatusAsync();
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Call an elevator");
                Console.WriteLine("2. Exit");

                var choice = Console.ReadLine();
                if (choice == "2") break;

                if (choice == "1")
                {
                    await HandleElevatorCallAsync(service);
                }
                else
                {
                    await DisplayErrorMessageAsync("Invalid choice. Please enter 1 or 2.");
                }
            }

            Console.WriteLine("Thank you for using the DVT Elevator Simulation!");
        }

        /// <summary>
        /// Handles the elevator call process.
        /// </summary>
        /// <param name="service">The elevator service instance.</param>
        private static async Task HandleElevatorCallAsync(ElevatorService service)
        {
            try
            {
                Console.Write("Enter destination floor number: ");
                if (!int.TryParse(Console.ReadLine(), out var destinationFloor) || destinationFloor < 1 || destinationFloor > 10)
                {
                    await DisplayErrorMessageAsync("Invalid destination floor number. Please enter a number between 1 and 10.");
                    return;
                }

                Console.Write("Enter number of passengers: ");
                if (!int.TryParse(Console.ReadLine(), out var passengers) || passengers < 1 || passengers > 15)
                {
                    await DisplayErrorMessageAsync("Elevator have exceeded the maximum carrying capacity. Please enter a number between 1 and 15.");
                    return;
                }


                await service.MoveElevatorToUserDestinationAsync(destinationFloor, passengers);
                await Task.Delay(4000); // Simulate real-time processing
            }
            catch (Exception ex)
            {
                await DisplayErrorMessageAsync($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays an error message and pauses for a specified duration.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        private static async Task DisplayErrorMessageAsync(string message)
        {
            Console.WriteLine(message);
            await Task.Delay(4000); // Pause to allow the user to read the message
        }
    }
}
