using Asteroids.Logic.Common.Services;
using Asteroids.Logic.FSMachine;
using Cysharp.Threading.Tasks;
using R3;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class FinalScoreViewModel
    {
        public ReadOnlyReactiveProperty<int> Score { get; private set; }
        public ReadOnlyReactiveProperty<int> BestScore { get; private set; }
        private FSM _fsm;
        private SceneService _sceneService;

        public FinalScoreViewModel(Score score, FSM fsm, SceneService sceneService)
        {
            Score = score.Current;
            BestScore = score.Best;
            _fsm = fsm;
            _sceneService = sceneService;
        }

        public void RestartButtonClickedHandler()
        {
            _fsm.SwitchState(StateEnum.Run);
        }

        public void ReturnToMenuButtonClickedHandler()
        {
            _sceneService.LoadMainMenuScene().Forget();
        }
    }
}