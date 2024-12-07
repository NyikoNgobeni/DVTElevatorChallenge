namespace DVTElevatorChallengeTest.Application.Interfaces
{
    public interface IElevatorService
    {
        Task DisplayStatusAsync();
        Task MoveElevatorToUserDestinationAsync(int destinationFloor, int passengers);
    }
}