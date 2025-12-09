using TMPro;
using UnityEngine;

namespace Asteroids.Logic.Common.UI.Core
{
    public abstract class Window<T> : MonoBehaviour, IWindow<T>
    {
        protected T _viewModel;

        public void Setup(T viewModel)
        {
            _viewModel = viewModel;
        }

        public abstract void Show(bool ignoreAnimation = false);
        public abstract void Hide(bool ignoreAnimation = false);
    }
}