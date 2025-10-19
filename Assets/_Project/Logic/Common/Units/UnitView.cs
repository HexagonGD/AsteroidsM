using Asteroids.Logic.Common.Units.Core;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Common.Units
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private bool _destroyCollisionWithEffect = true;

        public Unit Unit { get; set; }

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

        public class Factory : PlaceholderFactory<UnitView> { }
    }
}