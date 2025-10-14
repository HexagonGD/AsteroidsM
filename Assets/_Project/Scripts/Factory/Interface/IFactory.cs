namespace Asteroids.Factory.Interface
{
    public interface IFactory<T>
    {
        public T Get();
    }
}