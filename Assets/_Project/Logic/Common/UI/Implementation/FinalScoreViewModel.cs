using Asteroids.Logic.Common.Services;
using Asteroids.Logic.FSMachine;
using R3;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class FinalScoreViewModel
    {
        public ReadOnlyReactiveProperty<int> Score;
        private FSM _fsm;

        public FinalScoreViewModel(Score score, FSM fsm)
        {
            Score = score.Value;
            _fsm = fsm;
        }

        public void RestartButtonClickedHandler()
        {
            _fsm.SwitchState(StateEnum.Play);
        }
    }
}