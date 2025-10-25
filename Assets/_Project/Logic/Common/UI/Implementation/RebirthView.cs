using Asteroids.Logic.Common.UI.Core;
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

        [Inject]
        public void Construct(RebirthViewModel viewModel)
        {
            Setup(viewModel);
        }

        public void Initialize()
        {
            _watchAdButton.OnClickAsObservable().Subscribe(_ => _viewModel.ShowAd(Ads.Core.AdType.Rewarded)).AddTo(this);
            _skipButton.OnClickAsObservable().Subscribe(_ => _viewModel.ShowAd(Ads.Core.AdType.Interstitial)).AddTo(this);
            _viewModel.RewardedAdsAvailable.Subscribe(x => _watchAdButton.interactable = x).AddTo(this);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}