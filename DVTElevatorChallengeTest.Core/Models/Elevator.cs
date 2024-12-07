using DVTElevatorChallengeTest.Core.Models;

public class Elevator(int id)
{
    public int Id { get; } = id;
    public int CurrentFloor { get; set; } = 0;
    public ElevatorDirection Direction { get; set; } = GetRandomDirection();
    public ElevatorState State { get; set; } = GetRandomState();
    public bool IsMoving => State == ElevatorState.Moving;
    public int PassengerCount { get; set; } = GetRandomPassengerCount();
    public int MaxPassengers { get; } = ElevatorConstants.MaxPassengers;

    private static ElevatorDirection GetRandomDirection()
    {
        var random = new Random();
        var directions = Enum.GetValues<ElevatorDirection>();
        return (ElevatorDirection)directions.GetValue(random.Next(directions.Length));
    }

    private static ElevatorState GetRandomState()
    {
        var random = new Random();
        var states = Enum.GetValues<ElevatorState>();
        return (ElevatorState)states.GetValue(random.Next(states.Length));
    }

    private static int GetRandomPassengerCount()
    {
        var random = new Random();
        return random.Next(0, ElevatorConstants.MaxPassengers + 1);
    }
}
