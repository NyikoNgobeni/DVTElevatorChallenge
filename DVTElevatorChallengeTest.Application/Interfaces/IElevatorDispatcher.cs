using DVTElevatorChallengeTest.Core.Models;

namespace DVTElevatorChallengeTest.Application.Interfaces
{
    public interface IElevatorDispatcher
    {
        Task<Elevator> DispatchAsync(int requestedFloor, int passengers);
    }
}
