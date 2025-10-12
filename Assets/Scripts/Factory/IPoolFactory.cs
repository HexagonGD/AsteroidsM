public interface IPoolFactory<T> : IFactory<T>
{
    public void Release(T value);
}