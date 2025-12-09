using Asteroids.Logic.Common.UI.Core;
using Cysharp.Threading.Tasks;
using PrimeTween;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class RebirthView : Window<RebirthViewModel>, IInitializable
    {
        [SerializeField] private Button _watchAdButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private TweenSettings<float> _showAnimation;

        [Inject]
        public void Construct(RebirthViewModel viewModel)
        {
            Setup(viewModel);
        }

        public void Initialize()
        {
            _watchAdButton.OnClickAsObservable().Subscribe(_ => _viewModel.RequestRebirth().Forget()).AddTo(this);
            _skipButton.OnClickAsObservable().Subscribe(_ => _viewModel.SkipAsync().Forget()).AddTo(this);
            _watchAdButton.interactable = _viewModel.RewardedAdsAvailable.CurrentValue;
            _viewModel.RewardedAdsAvailable.Subscribe(x => _watchAdButton.interactable = x).AddTo(this);
        }

        public override void Show(bool ignoreAnimation = false)
        {
            if (gameObject.activeInHierarchy == true)
                return;

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