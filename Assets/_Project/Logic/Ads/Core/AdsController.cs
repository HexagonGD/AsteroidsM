using Asteroids.Logic.Common.Services.Saving.Core;
using Asteroids.Logic.Payments.Core;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Ads.Core
{
    public class AdsController : IInitializable, IDisposable
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

        private readonly ISaveSystem _saveManager;
        private readonly IAdsProvider _adsProvider;
        private readonly PaymentService _paymentService;
        private readonly ReactiveProperty<bool> _adsDisabled = new(false);

        private DisposableBag _disposable;

        public ReadOnlyReactiveProperty<bool> AdsDisabled => _adsDisabled;
        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable => _adsProvider.RewardedAdsAvailable;
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable => _adsProvider.InterstitialAdsAvailable;

        public AdsController(ISaveSystem saveManager, IAdsProvider adsProvider, PaymentService paymentService)
        {
            _saveManager = saveManager;
            _adsProvider = adsProvider;
            _paymentService = paymentService;
        }

        public void Initialize()
        {
            _saveManager.Data.Subscribe(x => _adsDisabled.Value = x.AdsDisabled).AddTo(ref _disposable);
            _paymentService.BoughtProducts.ObserveAdd().Where(x => x.Value == "disable_ads").Subscribe(_ => DisableAds()).AddTo(ref _disposable);
        }

        public void ShowAd(AdType adType, bool ignoreDisableAds = false)
        {
            if (ignoreDisableAds || AdsDisabled.CurrentValue == false)
                _adsProvider.ShowAd(adType);
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
        }
    }
}