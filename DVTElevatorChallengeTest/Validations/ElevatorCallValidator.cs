using FluentValidation;

namespace DVTElevatorChallengeTest.ConsoleApp.Validations
{
    public class ElevatorCallValidator : AbstractValidator<ElevatorCall>
    {
        private const int MaxCarryingCapacity = 15;
        private const int MinFloor = 1;
        private const int MaxFloor = 10;

        public ElevatorCallValidator()
        {
            RuleFor(x => x.DestinationFloor)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Destination floor number is required.")
                .Must(BeAValidFloor).WithMessage($"Invalid destination floor number. Please enter a number between {MinFloor} and {MaxFloor}.");

            RuleFor(x => x.Passengers)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Number of passengers is required.")
                .Must(BeAValidPassengerCount).WithMessage($"Please enter a number between 1 and {MaxCarryingCapacity}.")
                .Must(BeWithinCarryingCapacity).WithMessage($"Elevator capacity exceeded. Maximum allowed is {MaxCarryingCapacity}.");
        }

        private bool BeAValidFloor(string floor)
        {
            return int.TryParse(floor, out var floorNumber) && floorNumber >= MinFloor && floorNumber <= MaxFloor;
        }

        private bool BeAValidPassengerCount(string passengers)
        {
            return int.TryParse(passengers, out var passengerCount) && passengerCount >= 1;
        }

        private bool BeWithinCarryingCapacity(string passengers)
        {
            return int.TryParse(passengers, out var passengerCount) && passengerCount <= MaxCarryingCapacity;
        }
    }
}
