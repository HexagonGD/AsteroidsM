using Asteroids.Logic.Common.Services.Saving;
using Asteroids.Logic.Common.Services.Saving.Core;
using Asteroids.Logic.FSMachine;
using R3;

namespace Asteroids.Logic.Common.Services
{
    public class Score
    {
        private readonly ISaveManager _saveManager;
        private int _previousBest = 0;
        private ReactiveProperty<int> _best = new(0);

        public ReactiveProperty<int> Current = new ReactiveProperty<int>(0);
        public ReadOnlyReactiveProperty<int> Best => _best;

        public Score(ISaveManager saveManager, FSM fsm)
        {
            _saveManager = saveManager;

            if (_saveManager.TryLoad(out var data))
            {
                _previousBest = data.BestScore;
                _best.Value = data.BestScore;
            }

            Current.Where(x => x > _best.CurrentValue).Subscribe(x => _best.Value = x);
            fsm.OnStateChanged += FSMStateChangedHandler;
        }

        private void FSMStateChangedHandler(StateEnum state)
        {
            if (state == StateEnum.Score && _previousBest < _best.Value)
            {
                _saveManager.Save(new SaveData() { BestScore = _best.Value });
                _previousBest = _best.Value;
            }
        }
    }
}