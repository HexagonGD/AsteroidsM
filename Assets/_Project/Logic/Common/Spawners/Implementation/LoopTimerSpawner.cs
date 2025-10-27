using Asteroids.Logic.Common.Configs.Core;
using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Spawners.Core;
using System;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public abstract class LoopTimerSpawner<T> : ISpawner<T>
    {
        public event Action<T> OnSpawned;

        private readonly ILoopTimerSpawnerConfig _config;
        private LoopTimer _timer;

        public LoopTimerSpawner(ILoopTimerSpawnerConfig config)
        {
            _config = config;
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
            _timer = new LoopTimer(_config.TimeForSpawn, _config.AccumulatedTime);
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