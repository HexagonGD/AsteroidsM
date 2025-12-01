using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Bootstrap;
using Cysharp.Threading.Tasks;
using R3;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class RebirthViewModel
    {
        private readonly Game _game;
        private readonly AdsController _adsController;

        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable => _adsController.RewardedAdsAvailable;
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable => _adsController.InterstitialAdsAvailable;

        public RebirthViewModel(Game game, AdsController adsController)
        {
            _game = game;
            _adsController = adsController;
        }

        public async UniTask RequestRebirth()
        {
            var showResult = await _adsController.ShowRewardedAdAsync(true);
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
            await _adsController.ShowInterstitialAdAsync();
            _game.Complete();
        }
    }
}