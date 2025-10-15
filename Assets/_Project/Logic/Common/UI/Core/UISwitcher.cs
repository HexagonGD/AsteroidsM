using Asteroids.Logic.Common.UI.Implementation;
using Asteroids.Logic.FSMachine;

namespace Asteroids.Logic.Common.UI.Core
{
    public class UISwitcher
    {
        private readonly DebugView _debugView;
        private readonly FinalScoreView _finalScoreView;
        private readonly FSM _fsm;

        public UISwitcher(DebugView debugView, FinalScoreView finalScoreView, FSM fsm)
        {
            _debugView = debugView;
            _finalScoreView = finalScoreView;
            _fsm = fsm;

            _fsm.OnStateChanged += StateChangedHandler;
        }

        private void StateChangedHandler(StateEnum state)
        {
            if(state == StateEnum.Play)
            {
                _debugView.Show();
                _finalScoreView.Hide();
            }
            else
            {
                _debugView.Hide();
                _finalScoreView.Show();
            }
        }
    }
}