namespace Asteroids.Logic.Common.Services.Factory.Core
{
    public interface IPoolFactory<T> : IFactory<T>
    {
        public void Release(T value);
    }
}