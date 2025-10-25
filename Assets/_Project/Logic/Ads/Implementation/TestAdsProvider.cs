using Asteroids.Logic.Ads.Core;
using R3;
using System;

namespace Asteroids.Logic.Ads.Implementation
{
    public class TestAdsProvider : IAdsProvider
    {
        public event Action<AdType, AdShowResult> OnAdShowResult;

        private ReactiveProperty<bool> _rewarderAdsAvailable = new(true);
        private ReactiveProperty<bool> _interstitialAdsAvailable = new(true);

        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable => _rewarderAdsAvailable;
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable => _interstitialAdsAvailable;

        public void ShowAd(AdType adType)
        {
            OnAdShowResult?.Invoke(adType, AdShowResult.Success);
        }
    }
}