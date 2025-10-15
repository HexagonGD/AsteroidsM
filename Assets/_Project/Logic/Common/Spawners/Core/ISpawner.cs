using System;

namespace Asteroids.Logic.Common.Spawners.Core
{
    public interface ISpawner<T>
    {
        public event Action<T> OnSpawned;
        public void Update(float deltaTime);
        public void Clear();
    }
}