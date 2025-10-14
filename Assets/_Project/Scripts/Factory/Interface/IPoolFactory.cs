namespace Asteroids.Factory.Interface
{
    public interface IPoolFactory<T> : IFactory<T>
    {
        public void Release(T value);
    }
}