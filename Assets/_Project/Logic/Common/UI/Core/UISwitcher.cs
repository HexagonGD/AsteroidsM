using Asteroids.Logic.Common.UI.Implementation;
using Asteroids.Logic.FSMachine;
using Zenject;

namespace Asteroids.Logic.Common.UI.Core
{
    public class UISwitcher : IInitializable
    {
        private readonly DebugView _debugView;
        private readonly FinalScoreView _finalScoreView;
        private readonly RebirthView _rebirthView;
        private readonly FSM _fsm;

        public UISwitcher(DebugView debugView, FinalScoreView finalScoreView, RebirthView rebirthView, FSM fsm)
        {
            _debugView = debugView;
            _finalScoreView = finalScoreView;
            _rebirthView = rebirthView;
            _fsm = fsm;

            _fsm.OnStateChanged += StateChangedHandler;
        }

        public void Initialize()
        {
            _debugView.Hide(true);
            _finalScoreView.Hide(true);
            _rebirthView.Hide(true);
        }

        private void StateChangedHandler(StateEnum state)
        {
            if(state == StateEnum.Play)
            {
                _debugView.Show();
                _finalScoreView.Hide();
                _rebirthView.Hide();

            }
            else if(state == StateEnum.Rebirth)
            {
                _debugView.Hide();
                _finalScoreView.Hide();
                _rebirthView.Show();
            }
            else if(state == StateEnum.Score)
            {
                _debugView.Hide();
                _finalScoreView.Show();
                _rebirthView.Hide();
            }
        }
    }
}