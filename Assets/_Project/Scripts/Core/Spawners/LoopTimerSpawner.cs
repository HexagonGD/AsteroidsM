using Asteroids.Timers.Implementation;
using System;

namespace Asteroids.Spawners.Core
{
    public abstract class LoopTimerSpawner<T> : ISpawner<T>
    {
        public event Action<T> OnSpawned;

        private readonly float _timeForSpawn;
        private readonly float _accumulatedTime;
        private LoopTimer _timer;

        public LoopTimerSpawner(float timeForSpawn, float accumulatedTime)
        {
            _timeForSpawn = timeForSpawn;
            _accumulatedTime = accumulatedTime;

            Clear();
        }

        public void Update(float deltaTime)
        {
            _timer.Update(deltaTime);
        }

        public virtual void Clear()
        {
            if (_timer != null)
                _timer.OnLoop -= Spawn;
            _timer = new LoopTimer(_timeForSpawn, _accumulatedTime);
            _timer.OnLoop += Spawn;
        }

        public void Spawn()
        {
            var spawned = SpawnHandler();
            OnSpawned?.Invoke(spawned);
        }

        protected abstract T SpawnHandler();
    }
}