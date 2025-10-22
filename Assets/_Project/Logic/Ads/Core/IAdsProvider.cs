using System;

namespace Asteroids.Logic.Ads.Core
{
    public interface IAdsProvider
    {
        public event Action<AdType, AdShowResult> OnAdShowResult;

        public void ShowAd(AdType adType);
    }
}