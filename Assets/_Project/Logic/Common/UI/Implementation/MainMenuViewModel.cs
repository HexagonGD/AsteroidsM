using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Payments.Core;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class MainMenuViewModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IAdsService _adsService;
        private readonly SceneService _sceneService;

        public ReadOnlyReactiveProperty<bool> AdsDisabled => _adsService.AdsDisabled;

        public MainMenuViewModel(IPaymentService paymentService, IAdsService adsService, SceneService sceneService)
        {
            _paymentService = paymentService;
            _adsService = adsService;
            _sceneService = sceneService;
        }

        public void RunGame()
        {
            Debug.Log("RunGame");
            _sceneService.LoadGameScene().Forget();
        }

        public void DisableAds()
        {
            _paymentService.BuyProduct("disable_ads");
        }
    }
}