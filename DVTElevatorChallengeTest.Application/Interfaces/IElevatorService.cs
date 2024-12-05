namespace DVTElevatorChallengeTest.Application.Interfaces
{
    public interface IElevatorService
    {
        Task CallElevatorAsync(int floor, int passengers);
        Task DisplayStatusAsync();
        Task MoveElevatorToUserAndDestinationAsync(int userFloor, int destinationFloor, int passengers);
    }
}