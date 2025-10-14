using Asteroids.Core;
using Asteroids.FSMachine;
using R3;

namespace Asteroids.UI.Implementation
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