using TMPro;
using UnityEngine;

namespace Asteroids.UI.Core
{
    public abstract class Window<T> : MonoBehaviour, IWindow<T>
    {
        protected T _viewModel;

        public void Setup(T viewModel)
        {
            _viewModel = viewModel;
        }

        public abstract void Show();
        public abstract void Hide();
    }
}