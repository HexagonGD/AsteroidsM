using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Payments.Core;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class MainMenuViewModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IAdsService _adsService;

        public ReadOnlyReactiveProperty<bool> AdsDisabled => _adsService.AdsDisabled;

        public MainMenuViewModel(IPaymentService paymentService, IAdsService adsService)
        {
            _paymentService = paymentService;
            _adsService = adsService;
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