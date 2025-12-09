namespace Asteroids.Logic.Common.UI.Core
{
    public interface IWindow<in T> : IWindow
    {
        public void Setup(T viewModel);
    }

    public interface IWindow
    {
        public void Show(bool ignoreAnimation = false);
        public void Hide(bool ignoreAnimation = false);
    }
}