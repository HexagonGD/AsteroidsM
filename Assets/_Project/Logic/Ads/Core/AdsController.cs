using R3;
using System;

namespace Asteroids.Logic.Ads.Core
{
    public class AdsController
    {
        public event Action<AdType, AdShowResult> OnAdShowed
        {
            add
            {
                _adsProvider.OnAdShowResult += value;
            }
            remove
            {
                _adsProvider.OnAdShowResult -= value;
            }
        }

        private readonly IAdsProvider _adsProvider;

        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable => _adsProvider.RewardedAdsAvailable;
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable => _adsProvider.InterstitialAdsAvailable;

        public AdsController(IAdsProvider adsProvider)
        {
            _adsProvider = adsProvider;
        }

        public void ShowAd(AdType adType)
        {
            _adsProvider.ShowAd(adType);
        }
    }
}