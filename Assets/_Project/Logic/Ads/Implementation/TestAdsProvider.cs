using Asteroids.Logic.Ads.Core;
using System;

namespace Asteroids.Logic.Ads.Implementation
{
    public class TestAdsProvider : IAdsProvider
    {
        public event Action<AdType, AdShowResult> OnAdShowResult;

        public void ShowAd(AdType adType)
        {
            OnAdShowResult?.Invoke(adType, AdShowResult.Success);
        }
    }
}