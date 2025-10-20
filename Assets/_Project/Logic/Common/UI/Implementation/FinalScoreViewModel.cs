using Asteroids.Logic.Common.Services;
using Asteroids.Logic.FSMachine;
using R3;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class FinalScoreViewModel
    {
        public ReadOnlyReactiveProperty<int> Score;
        public ReadOnlyReactiveProperty<int> BestScore;
        private FSM _fsm;

        public FinalScoreViewModel(Score score, FSM fsm)
        {
            Score = score.Current;
            BestScore = score.Best;
            _fsm = fsm;
        }

        public void RestartButtonClickedHandler()
        {
            _fsm.SwitchState(StateEnum.Play);
        }
    }
}