using Asteroids.Logic.Common.UI.Core;
using PrimeTween;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class FinalScoreView : Window<FinalScoreViewModel>
    {
        [SerializeField] private TMP_Text _scoreTMP;
        [SerializeField] private TMP_Text _bestScoreTMP;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _returnToMenuButton;
        [SerializeField] private TweenSettings<float> _showAnimation;

        [Inject]
        public void Construct(FinalScoreViewModel viewModel)
        {
            Setup(viewModel);
        }

        private void Start()
        {
            _restartButton.OnClickAsObservable().Subscribe(_ => _viewModel.RestartButtonClickedHandler()).AddTo(this);
            _returnToMenuButton.OnClickAsObservable().Subscribe(_ => _viewModel.ReturnToMenuButtonClickedHandler()).AddTo(this);
        }

        public override void Show(bool ignoreAnimation = false)
        {
            if (gameObject.activeInHierarchy == true)
                return;

            _scoreTMP.SetText($"Score: {_viewModel.Score.CurrentValue}");
            _bestScoreTMP.SetText($"Best: {_viewModel.BestScore.CurrentValue}");
            gameObject.SetActive(true);

            var tween = Tween.UIAnchoredPositionY(transform as RectTransform, _showAnimation);
            if (ignoreAnimation)
                tween.Complete();
        }

        public override void Hide(bool ignoreAnimation = false)
        {
            if (gameObject.activeInHierarchy == false)
                return;

            var tween = Tween.UIAnchoredPositionY(transform as RectTransform, _showAnimation.WithDirection(false))
                .OnComplete(target: this, target => target.gameObject.SetActive(false), warnIfTargetDestroyed: false);
            if(ignoreAnimation)
                tween.Complete();
        }
    }
}