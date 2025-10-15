namespace Asteroids.Logic.Common.Services.Factory.Core
{
    public interface IFactory<T>
    {
        public T Get();
    }
}