using Asteroids.Logic.Analytics.Core;
using Asteroids.Logic.FSMachine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Logic.Analytics.Implementation
{
    public class StartGameAnalyticListener : IAnalyticListener
    {
        public event Action<string, IEnumerable<AnalyticParameter>> OnEventListened;

        private readonly FSM _fsm;

        public StartGameAnalyticListener(FSM fsm)
        {
            _fsm = fsm;
            _fsm.OnStateChanged += StateChangedHandler;
        }

        private void StateChangedHandler(StateEnum state)
        {
            if (state == StateEnum.Play)
                OnEventListened?.Invoke(EventNames.StartGameEvent, Enumerable.Empty<AnalyticParameter>());
        }
    }
}