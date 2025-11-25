using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Bootstrap;
using R3;
using System;
using Zenject;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class RebirthViewModel : IInitializable, IDisposable
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

        public void Initialize()
        {
            _adsController.OnAdShowed += AdShowedHandler;
        }

        public void ShowAd(AdType adType, bool ignoreDisableAds = false)
        {

        }

        public void RequestRebirth()
        {
            _adsController.ShowAd(AdType.Rewarded, true);
        }

        public void Skip()
        {
            _adsController.ShowAd(AdType.Interstitial);
            _game.Complete();
        }

        private void AdShowedHandler(AdType adType, AdShowResult result)
        {
            if (adType == AdType.Rewarded && result == AdShowResult.Success)
                _game.Rebirth();
        }

        public void Dispose()
        {
            _adsController.OnAdShowed -= AdShowedHandler;
        }
    }
}