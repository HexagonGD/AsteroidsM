using Asteroids.Logic.Common.Units.Core;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Common.Units
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private bool _destroyCollisionWithEffect = true;
        [SerializeField] private GameObject _dieEffect;

        private Unit _unit;

        public Unit Unit
        {
            get => _unit;
            set
            {
                if (_unit != null)
                    _unit.OnDied -= DiedHandler;
                value.OnDied += DiedHandler;
                _unit = value;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<UnitView>(out var unitView))
            {
                unitView.Unit?.Die(_destroyCollisionWithEffect);
            }
        }

        private void DiedHandler(Unit unit, bool real)
        {
            if (real && _dieEffect != null)
                UnityEngine.Object.Instantiate(_dieEffect, transform.position, Quaternion.identity);
        }

        public class Factory : PlaceholderFactory<UnitView> { }
    }
}