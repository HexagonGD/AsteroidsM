using Asteroids.Logic.Common.Services.Saving;
using Asteroids.Logic.Common.Services.Saving.Core;
using Asteroids.Logic.FSMachine;
using R3;
using System;

namespace Asteroids.Logic.Common.Services
{
    public class Score : IDisposable
    {
        private readonly ISaveManager _saveManager;
        private readonly FSM _fsm;
        private int _previousBest = 0;
        private ReactiveProperty<int> _best = new(0);

        public ReactiveProperty<int> Current = new ReactiveProperty<int>(0);
        public ReadOnlyReactiveProperty<int> Best => _best;

        public Score(ISaveManager saveManager, FSM fsm)
        {
            _saveManager = saveManager;
            _fsm = fsm;

            if (_saveManager.TryLoad(out var data))
            {
                _previousBest = data.BestScore;
                _best.Value = data.BestScore;
            }

            Current.Where(x => x > _best.CurrentValue).Subscribe(x => _best.Value = x);
            _fsm.OnStateChanged += FSMStateChangedHandler;
        }

        private void FSMStateChangedHandler(StateEnum state)
        {
            if (state == StateEnum.Score && _previousBest < _best.Value)
            {
                _saveManager.Save(new SaveData() { BestScore = _best.Value });
                _previousBest = _best.Value;
            }
        }

        public void Dispose()
        {
            _fsm.OnStateChanged -= FSMStateChangedHandler;
        }
    }
}