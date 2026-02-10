using Asteroids.Logic.Common.UI.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using R3;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class MainMenuView : Window<MainMenuViewModel>, IInitializable
    {
        [SerializeField] private Button _runGameButton;
        [SerializeField] private Button _disableAdsButton;

        [Inject]
        public void Construct(MainMenuViewModel viewModel)
        {
            Setup(viewModel);
        }

        public void Initialize()
        {
            _disableAdsButton.interactable = !_viewModel.AdsDisabled.CurrentValue;
            _viewModel.AdsDisabled.Subscribe(x => _disableAdsButton.interactable = !x).AddTo(this);
            _runGameButton.OnClickAsObservable().Subscribe(_ => _viewModel.RunGame()).AddTo(this);
            _disableAdsButton.OnClickAsObservable().Subscribe(_ => _viewModel.DisableAds()).AddTo(this);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}