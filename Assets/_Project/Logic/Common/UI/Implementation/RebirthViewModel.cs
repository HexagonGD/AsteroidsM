using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Bootstrap;
using Cysharp.Threading.Tasks;
using R3;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class RebirthViewModel
    {
        private readonly Game _game;
        private readonly IAdsService _adsService;

        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable => _adsService.RewardedAdsAvailable;
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable => _adsService.InterstitialAdsAvailable;

        public RebirthViewModel(Game game, IAdsService adsService)
        {
            _game = game;
            _adsService = adsService;
        }

        public async UniTask RequestRebirth()
        {
            var showResult = await _adsService.ShowRewardedAdAsync(true);
            switch (showResult)
            {
                case AdShowResult.Success:
                case AdShowResult.AdsDisabled:
                    _game.Rebirth();
                    break;
                case AdShowResult.Failed:
                case AdShowResult.Canceled:
                case AdShowResult.NoAds:
                    break;
            }
        }

        public async UniTask SkipAsync()
        {
            await _adsService.ShowInterstitialAdAsync();
            _game.Complete();
        }
    }
}