using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Common.Services.Saving.Core;
using Asteroids.Logic.Payments.Core;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Ads.Implementation
{
    public class AdsService : IAdsService, IInitializable, IDisposable
    {
        private readonly ISaveService _saveManager;
        private readonly IAdsProvider _adsProvider;
        private readonly PaymentService _paymentService;
        private readonly ReactiveProperty<bool> _adsDisabled = new(false);

        private UniTaskCompletionSource<AdShowResult> _rewardedShowTask;
        private UniTaskCompletionSource<AdShowResult> _interstitialShowTask;

        private DisposableBag _disposable;

        public ReadOnlyReactiveProperty<bool> AdsDisabled => _adsDisabled;
        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable => _adsProvider.RewardedAdsAvailable;
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable => _adsProvider.InterstitialAdsAvailable;

        public AdsService(ISaveService saveManager, IAdsProvider adsProvider, PaymentService paymentService)
        {
            _saveManager = saveManager;
            _adsProvider = adsProvider;
            _paymentService = paymentService;
        }

        public void Initialize()
        {
            _saveManager.Data.Subscribe(x => _adsDisabled.Value = x.AdsDisabled).AddTo(ref _disposable);
            _paymentService.BoughtProducts.ObserveAdd().Where(x => x.Value == "disable_ads").Subscribe(_ => DisableAds()).AddTo(ref _disposable);
            _adsProvider.OnAdShowResult += AdShowResultHandler;
        }

        public UniTask<AdShowResult> ShowRewardedAdAsync(bool ignoreDisableAds = false)
        {
            return ShowAd(ref _rewardedShowTask, AdType.Rewarded, ignoreDisableAds);
        }

        public UniTask<AdShowResult> ShowInterstitialAdAsync(bool ignoreDisableAds = false)
        {
            return ShowAd(ref _interstitialShowTask, AdType.Interstitial, ignoreDisableAds);
        }

        private UniTask<AdShowResult> ShowAd(ref UniTaskCompletionSource<AdShowResult> tcs, AdType adType, bool ignoreDisableAds)
        {
            if (ignoreDisableAds || AdsDisabled.CurrentValue == false)
            {
                if (tcs != null && tcs.GetStatus(0) == UniTaskStatus.Pending)
                    return tcs.Task;

                tcs = new UniTaskCompletionSource<AdShowResult>();

                _adsProvider.ShowAd(adType);
                return tcs.Task;
            }
            else
            {
                return UniTask.FromResult(AdShowResult.AdsDisabled);
            }
        }

        private void AdShowResultHandler(AdType type, AdShowResult result)
        {
            if (type == AdType.Rewarded)
                _rewardedShowTask?.TrySetResult(result);
            else if (type == AdType.Interstitial)
                _interstitialShowTask?.TrySetResult(result);
        }

        private void DisableAds()
        {
            if (_adsDisabled.Value)
            {
                Debug.Log("Trying to disable ads again");
            }
            else
            {
                _adsDisabled.Value = true;
                var data = _saveManager.Data.CurrentValue;
                data.AdsDisabled = true;
                _saveManager.SaveAsync(data).Forget();
            }
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _adsProvider.OnAdShowResult -= AdShowResultHandler;
        }
    }
}