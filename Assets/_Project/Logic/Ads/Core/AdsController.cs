using System;

namespace Asteroids.Logic.Ads.Core
{
    public class AdsController
    {
        public event Action<AdType, AdShowResult> OnAdShowResult
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