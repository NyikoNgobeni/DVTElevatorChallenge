using DVTElevatorChallengeTest.Application.Services;
using DVTElevatorChallengeTest.ConsoleApp.Validations;
using DVTElevatorChallengeTest.Infrastructure.Repositories;
using FluentValidation;

using Microsoft.Extensions.Logging;

namespace DVTElevatorChallengeTest.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(static builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<ElevatorService>();
            var elevator = new Elevator(); // Create an instance of Elevator
            var elevatorRepository = new ElevatorRepository(elevator); // Pass the instance to the constructor
            var service = new ElevatorService((Logger<ElevatorService>)logger, elevatorRepository);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the DVT Elevator Simulation!");
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
            var validator = new ElevatorCallValidator();
            // Collect and validate destination floor number
            string destinationFloorInput = await CollectAndValidateInputAsync(
                "Enter destination floor number: ",
                validator,
                nameof(ElevatorCall.DestinationFloor)
            );

            // Collect and validate number of passengers
            string passengersInput = await CollectAndValidateInputAsync(
                "Enter number of passengers: ",
                validator,
                nameof(ElevatorCall.Passengers)
            );

            // Proceed with validated inputs
            var destinationFloor = int.Parse(destinationFloorInput);
            var passengers = int.Parse(passengersInput);

            await SimulateElevatorProcessAsync("Elevator is on its way!", 2000);
            await SimulateElevatorProcessAsync("Doors Opening, You may Enter!", 2000);
            await SimulateElevatorProcessAsync("Doors Closing, Stay Safe!", 2000);
            await Task.Delay(2000); // Simulate elevator delay
            await service.MoveElevatorToUserDestinationAsync(destinationFloor, passengers);
            await Task.Delay(4000); // Simulate real-time processing
        }
        
        /// <summary>
        /// Collects and validates user input based on the specified property.
        /// </summary>
        /// <param name="prompt">The prompt message to display to the user.</param>
        /// <param name="validator">The validator instance for validating input.</param>
        /// <param name="propertyName">The property name to validate.</param>
        /// <returns>The validated user input.</returns>
        private static async Task<string> CollectAndValidateInputAsync(string prompt, IValidator<ElevatorCall> validator, string propertyName)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                var elevatorCall = new ElevatorCall();
                if (propertyName == nameof(ElevatorCall.DestinationFloor))
                {
                    elevatorCall.DestinationFloor = input;
                }
                else if (propertyName == nameof(ElevatorCall.Passengers))
                {
                    elevatorCall.Passengers = input;
                }

                var validationResult = await Task.Run(() => validator.Validate(elevatorCall, options => options.IncludeProperties(propertyName)));

                if (validationResult.IsValid)
                {
                    return input;
                }

                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
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
        private static async Task SimulateElevatorProcessAsync(string message, int delay)
        {
            Console.WriteLine(message);
            await Task.Delay(delay);
        }
    }
}
