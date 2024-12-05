namespace DVTElevatorChallengeTest.Core.Models
{
    using System;
    using System.Threading.Tasks;

    public class Elevator(int id)
    {
        public int Id { get; } = id;
        public int CurrentFloor { get; set; } = 0;
        public ElevatorDirection Direction { get; set; } = ElevatorDirection.Stationary;
        public ElevatorState State { get; set; } = ElevatorState.Idle;
        public bool IsMoving => State == ElevatorState.Moving;
        public int PassengerCount { get; set; } = 0;
        public int MaxPassengers { get; } = ElevatorConstants.MaxPassengers;
    }

}
