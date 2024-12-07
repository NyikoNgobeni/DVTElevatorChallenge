using DVTElevatorChallengeTest.Core.Models;

namespace DVTElevatorChallengeTest.Application.Interfaces
{
    public interface IElevatorRepository
    {
        Task<List<Elevator>> GetElevatorsAsync(int elevatorCount);
        Task MoveToFloorAsync(int floor);
        Task<bool> AddPassengersAsync(int passengers);
        Task MoveElevatorToUserDestinationAsync(int destinationFloor);
    }
}