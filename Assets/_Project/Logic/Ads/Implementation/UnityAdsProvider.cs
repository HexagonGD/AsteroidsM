using Asteroids.Logic.Ads.Core;
using R3;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Asteroids.Logic.Ads.Implementation
{
    public class UnityAdsProvider : MonoBehaviour, IAdsProvider, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public event Action<AdType, AdShowResult> OnAdShowResult;

        [SerializeField] private string _gameID;
        [SerializeField] private bool _testMode;

        [SerializeField] private string _rewarderID;
        [SerializeField] private string _interstitialID;

        private readonly ReactiveProperty<bool> _rewarderAvailable = new(false);
        private readonly ReactiveProperty<bool> _interstitialAvailable = new(false);

        public ReadOnlyReactiveProperty<bool> RewardedAdsAvailable => _rewarderAvailable;
        public ReadOnlyReactiveProperty<bool> InterstitialAdsAvailable => _interstitialAvailable;

        private void Awake()
        {
            Advertisement.Initialize(_gameID, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            Advertisement.Load(_rewarderID, this);
            Advertisement.Load(_interstitialID, this);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError(message);
        }

        public void ShowAd(AdType adType)
        {
            switch (adType)
            {
                case AdType.Rewarded:
                    Advertisement.Show(_rewarderID, this);
                    break;
                case AdType.Interstitial:
                    Advertisement.Show(_interstitialID, this);
                    break;
            }
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId == _rewarderID)
                _rewarderAvailable.Value = true;
            else if (placementId == _interstitialID)
                _interstitialAvailable.Value = true;
            else
                Debug.LogError($"Incorrect ad id {placementId}");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Advertisement.Load(_rewarderID, this);
        }

        public void OnUnityAdsShowStart(string placementId)
        {

        }

        public void OnUnityAdsShowClick(string placementId)
        {

        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _rewarderID)
            {
                OnAdShowResult?.Invoke(AdType.Rewarded, AdShowResult.Success);
                _rewarderAvailable.Value = false;
                Advertisement.Load(placementId, this);
            }
            else if (placementId == _interstitialID)
            {
                OnAdShowResult?.Invoke(AdType.Interstitial, AdShowResult.Success);
                _interstitialAvailable.Value = false;
                Advertisement.Load(placementId, this);
            }
            else
                Debug.LogError($"Incorrect ad id {placementId}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            if (placementId == _rewarderID)
            {
                OnAdShowResult?.Invoke(AdType.Rewarded, AdShowResult.Failed);
                _rewarderAvailable.Value = false;
                Advertisement.Load(placementId, this);
            }
            else if (placementId == _interstitialID)
            {
                OnAdShowResult?.Invoke(AdType.Interstitial, AdShowResult.Failed);
                _interstitialAvailable.Value = false;
                Advertisement.Load(placementId, this);
            }
            else
                Debug.LogError($"Incorrect ad id {placementId}");
        }
    }
}