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
            var service = new ElevatorService(dispatcher, logger, elevatorRepository);

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

        private static async Task HandleElevatorCallAsync(ElevatorService service)
        {
            try
            {
                Console.Write("Enter your current floor number: ");
                if (!int.TryParse(Console.ReadLine(), out var userFloor) || userFloor < 1 || userFloor > 10)
                {
                    await DisplayErrorMessageAsync("Invalid floor number. Please enter a number between 1 and 10.");
                    return;
                }

                Console.Write("Enter your destination floor number: ");
                if (!int.TryParse(Console.ReadLine(), out var destinationFloor) || destinationFloor < 1 || destinationFloor > 10)
                {
                    await DisplayErrorMessageAsync("Invalid floor number. Please enter a number between 1 and 10.");
                    return;
                }

                Console.Write("Enter number of passengers: ");
                if (!int.TryParse(Console.ReadLine(), out var passengers) || passengers < 1)
                {
                    await DisplayErrorMessageAsync("Invalid number of passengers. Please enter a positive number.");
                    return;
                }

                await service.MoveElevatorToUserAndDestinationAsync(userFloor, destinationFloor, passengers);
                await Task.Delay(4000); // Simulate real-time processing
            }
            catch (Exception ex)
            {
                await DisplayErrorMessageAsync($"An error occurred: {ex.Message}");
            }
        }

        private static async Task DisplayErrorMessageAsync(string message)
        {
            Console.WriteLine(message);
            await Task.Delay(4000); // Pause to allow the user to read the message
        }
    }
}
