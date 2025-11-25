using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Payments.Core;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class MainMenuViewModel
    {
        private readonly PaymentService _paymentService;
        private readonly AdsController _adsController;

        public ReadOnlyReactiveProperty<bool> AdsDisabled => _adsController.AdsDisabled;

        public MainMenuViewModel(PaymentService paymentService, AdsController adsController)
        {
            _paymentService = paymentService;
            _adsController = adsController;
        }

        public void RunGame()
        {
            Debug.Log("RunGame");
            SceneManager.LoadScene("Game");
        }

        public void DisableAds()
        {
            _paymentService.BuyProduct("disable_ads");
        }
    }
}