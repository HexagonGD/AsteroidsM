using Asteroids.Logic.Common.Services.Saving;
using Asteroids.Logic.Common.UI.Core;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class SaveResolveView : Window<SaveResolveViewModel>
    {
        [SerializeField] private SaveDataView _localDataView;
        [SerializeField] private SaveDataView _cloudDataView;

        [SerializeField] private CanvasGroup _waitResolvingGroup;

        [Inject]
        public void Construct(SaveResolveViewModel viewModel)
        {
            Setup(viewModel);
            _viewModel.NeedResolve.Where(x => x).Subscribe(_ => Show()).AddTo(this);
            _viewModel.NeedResolve.Where(x => !x).Subscribe(_ => Hide()).AddTo(this);
            _viewModel.Resolving.Subscribe(x =>
            {
                _waitResolvingGroup.alpha = x ? 1 : 0;
                _waitResolvingGroup.blocksRaycasts = x;
            }).AddTo(this);
        }

        private void Awake()
        {
            _localDataView.OnDataChoiced += DataChoicedHandler;
            _cloudDataView.OnDataChoiced += DataChoicedHandler;
            Show();
        }

        private void OnDestroy()
        {
            _localDataView.OnDataChoiced -= DataChoicedHandler;
            _cloudDataView.OnDataChoiced -= DataChoicedHandler;
        }

        public override void Show()
        {
            if (_viewModel != null && _viewModel.NeedResolve.CurrentValue && _viewModel.Resolving.CurrentValue == false)
            {
                _localDataView.Show(_viewModel.LocalData);
                _cloudDataView.Show(_viewModel.CloudData);

                gameObject.SetActive(true);
            }
            else
            {
                Hide();
            }
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }


        private void DataChoicedHandler(SaveData data)
        {
            _viewModel.Resolve(data).Forget();
        }
    }
}