using Asteroids.Factory.Implementation;
using Asteroids.Timers.Implementation;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Asteroids;

namespace Asteroids.Core
{
    public class UFOSystem
    {
        public event Action OnUFODied;

        private CompositeFactory _ufoFactory;

        private Config _config;
        private LoopTimer _timer;
        private PlayZone _playZone;
        private SpawnOutsideGameZone _spawnPosition;

        private List<CompositeUnit> _ufo = new();

        public UFOSystem(CompositeFactory ufoFactory, Config config, Unit target, PlayZone playZone, SpawnOutsideGameZone spawnPosition)
        {
            _ufoFactory = ufoFactory;
            _config = config;
            _playZone = playZone;
            _spawnPosition = spawnPosition;

            _timer = new LoopTimer(config.TimeForSpawn, config.AccumulatedTime);
            _timer.OnLoop += SpawnUFO;
        }

        public void Update(float deltaTime)
        {
            _timer.Update(deltaTime);
            for (var i = _ufo.Count - 1; i >= 0; i--)
            {
                _ufo[i].Update(deltaTime);
            }
        }

        public void Clear()
        {
            for (var i = _ufo.Count - 1; i >= 0; i--)
                _ufo[i].Unit.Die(false);

            _timer.OnLoop -= SpawnUFO;
            _timer = new LoopTimer(_config.TimeForSpawn, _config.AccumulatedTime);
            _timer.OnLoop += SpawnUFO;
        }

        private void SpawnUFO()
        {
            var ufo = _ufoFactory.Get();
            var data = new TransformData();
            data.Position = _spawnPosition.GetSpawnPosition(_playZone);
            ufo.Unit.Data = data;
            ufo.ForceUpdateTransform();
            ufo.Unit.OnDied += OnUFODead;
            _ufo.Add(ufo);
        }

        private void OnUFODead(Unit unit, bool real)
        {
            var index = _ufo.FindIndex(x => x.Unit == unit);
            _ufo[index].Unit.OnDied -= OnUFODead;
            _ufoFactory.Release(_ufo[index]);
            _ufo.RemoveAt(index);

            if (real)
                OnUFODied?.Invoke();
        }

        [Serializable]
        public class Config
        {
            [Min(0)] public float TimeForSpawn;
            public float AccumulatedTime;
        }
    }
}