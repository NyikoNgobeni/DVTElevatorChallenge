
namespace DVTElevatorChallengeTest.Core.Models
{
    public static class ElevatorConstants
    {
        public const int MaxPassengers = 10;
        public const int TotalFloors = 5;
    }

    public enum ElevatorDirection
    {
        Stationary,
        Up,
        Down
    }

    public enum ElevatorState
    {
        Moving,
        Idle
    }
}