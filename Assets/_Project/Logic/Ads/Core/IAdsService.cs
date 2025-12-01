using Cysharp.Threading.Tasks;
using R3;

namespace Asteroids.Logic.Ads.Core
{
    public interface IAdsService
    {
        public ReadOnlyReactiveProperty<bool> AdsDisabled { get; }
        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable { get; }
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable { get; }

        public UniTask<AdShowResult> ShowRewardedAdAsync(bool ignoreDisableAds = false);
        public UniTask<AdShowResult> ShowInterstitialAdAsync(bool ignoreDisableAds = false);
    }
}