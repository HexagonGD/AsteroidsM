namespace Asteroids.UI.Core
{
    public interface IWindow<in T> : IWindow
    {
        public void Setup(T viewModel);
    }

    public interface IWindow
    {
        public void Show();
        public void Hide();
    }
}