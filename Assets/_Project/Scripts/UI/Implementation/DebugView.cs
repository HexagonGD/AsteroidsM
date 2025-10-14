using UnityEngine;
using Asteroids.UI.Core;
using Zenject;
using TMPro;
using R3;

namespace Asteroids.UI.Implementation
{
    public class DebugView : Window<DebugViewModel>
    {
        [SerializeField] private TMP_Text _scoreTMP;
        [SerializeField] private TMP_Text _positionTMP;
        [SerializeField] private TMP_Text _rotationTMP;
        [SerializeField] private TMP_Text _chargesTMP;
        [SerializeField] private TMP_Text _reloadTMP;

        [Inject]
        public void Construct(DebugViewModel viewModel)
        {
            Setup(viewModel);
        }

        private void Start()
        {
            _viewModel.Score.Subscribe(x => _scoreTMP.SetText($"Score: {x}")).AddTo(this);
            _viewModel.Position.Subscribe(vector => _positionTMP.SetText($"Position: {vector.x:0.00} {vector.y:0.00}")).AddTo(this);
            _viewModel.Rotation.Subscribe(x => _rotationTMP.SetText($"Rotation: {x:0.00}")).AddTo(this);
            _viewModel.LazerCharges.Subscribe(x => _chargesTMP.SetText($"Charges: {x}")).AddTo(this);
            _viewModel.ChargeReload.Subscribe(x => _reloadTMP.SetText($"Reload: {x:0.00}")).AddTo(this);
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