using Asteroids.Logic.Common.Services.Saving.Core;
using Asteroids.Logic.FSMachine;
using Cysharp.Threading.Tasks;
using R3;
using System;
using Zenject;

namespace Asteroids.Logic.Common.Services
{
    public class Score : IInitializable, IDisposable
    {
        private readonly ISaveSystem _saveManager;
        private readonly FSM _fsm;
        private int _previousBest = 0;
        private ReactiveProperty<int> _best = new(0);

        public ReactiveProperty<int> Current = new ReactiveProperty<int>(0);
        public ReadOnlyReactiveProperty<int> Best => _best;

        public Score(ISaveSystem saveManager, FSM fsm)
        {
            _saveManager = saveManager;
            _fsm = fsm;
        }

        public void Initialize()
        {
            _previousBest = _saveManager.Data.CurrentValue.BestScore;
            _best.Value = _saveManager.Data.CurrentValue.BestScore;

            Current.Where(x => x > _best.CurrentValue).Subscribe(x => _best.Value = x);
            _fsm.OnStateChanged += FSMStateChangedHandler;
        }

        public void Dispose()
        {
            _fsm.OnStateChanged -= FSMStateChangedHandler;
        }

        private void FSMStateChangedHandler(StateEnum state)
        {
            if (state == StateEnum.Score && _previousBest < _best.Value)
            {
                var data = _saveManager.Data.CurrentValue;
                data.BestScore = _best.Value;
                _saveManager.SaveAsync(data).Forget();
                _previousBest = _best.Value;
            }
        }
    }
}