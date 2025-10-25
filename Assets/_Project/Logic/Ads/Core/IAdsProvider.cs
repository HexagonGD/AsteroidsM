using R3;
using System;

namespace Asteroids.Logic.Ads.Core
{
    public interface IAdsProvider
    {
        public event Action<AdType, AdShowResult> OnAdShowResult;

        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable { get; }
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable { get; }

        public void ShowAd(AdType adType);
    }
}