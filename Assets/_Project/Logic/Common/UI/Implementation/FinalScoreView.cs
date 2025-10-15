using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using R3;
using Asteroids.Logic.Common.UI.Core;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class FinalScoreView : Window<FinalScoreViewModel>
    {
        [SerializeField] private TMP_Text _scoreTMP;
        [SerializeField] private Button _restartButton;

        [Inject]
        public void Construct(FinalScoreViewModel viewModel)
        {
            Setup(viewModel);
        }

        private void Start()
        {
            _restartButton.OnClickAsObservable().Subscribe(_ => _viewModel.RestartButtonClickedHandler()).AddTo(this);
        }

        public override void Show()
        {
            _scoreTMP.SetText($"Score: {_viewModel.Score.CurrentValue}");
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}