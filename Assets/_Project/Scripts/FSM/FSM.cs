using System;

namespace Asteroids.FSMachine
{
    public class FSM
    {
        public event Action<StateEnum> OnStateChanged;

        public StateEnum State { get; private set; }

        public void SwitchState(StateEnum state)
        {
            if (State == state)
                return;

            State = state;
            OnStateChanged?.Invoke(State);
        }
    }
}