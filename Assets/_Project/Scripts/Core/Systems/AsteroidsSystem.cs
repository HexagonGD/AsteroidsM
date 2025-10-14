using Asteroids.Factory.Implementation;
using Asteroids.Timers.Implementation;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Asteroids.Core
{
    public class AsteroidsSystem
    {
        public event Action OnBigAsteroidDied;
        public event Action OnSmallAsteroidDied;

        private CompositeFactory _smallAsteroidFactory;
        private CompositeFactory _bigAsteroidFactory;

        private Config _config;
        private LoopTimer _timer;
        private PlayZone _playZone;
        private SpawnOutsideGameZone _spawnPosition;
        private DeadZone _deadZone = new();

        private List<CompositeUnit> _asteroids = new();

        public AsteroidsSystem([Inject(Id = "small")] CompositeFactory smallAsteroidFactory, [Inject(Id = "big")] CompositeFactory bigAsteroidFactory,
                                Config config, PlayZone playZone, SpawnOutsideGameZone spawnPosition)
        {
            _config = config;
            _playZone = playZone;
            _spawnPosition = spawnPosition;

            _timer = new LoopTimer(config.TimeForSpawn, config.AccumulatedTime);
            _timer.OnLoop += SpawnBigAsteroid;

            _smallAsteroidFactory = smallAsteroidFactory;
            _bigAsteroidFactory = bigAsteroidFactory;
        }

        public void Update(float deltaTime)
        {
            _timer.Update(deltaTime);
            for (var i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Update(deltaTime);
                if (_deadZone.CheckInDeadZone(_asteroids[i].Unit.Data, _playZone))
                    _asteroids[i].Unit.Die(false);
            }
        }

        public void Clear()
        {
            for (var i = _asteroids.Count - 1; i >= 0; i--)
                _asteroids[i].Unit.Die(false);

            _timer.OnLoop -= SpawnBigAsteroid;
            _timer = new LoopTimer(_config.TimeForSpawn, _config.AccumulatedTime);
            _timer.OnLoop += SpawnBigAsteroid;
        }

        private void SpawnBigAsteroid()
        {
            var asteroid = _bigAsteroidFactory.Get();
            var data = new TransformData();

            data.Position = _spawnPosition.GetSpawnPosition(_playZone);

            var x = Random.Range(-_playZone.Width / 4f, _playZone.Width / 4f);
            var y = Random.Range(-_playZone.Height / 4f, _playZone.Height / 4f);

            data.Speed = (new Vector2(x, y) - data.Position).normalized * Random.Range(_config.BigAsteroidMinSpeed, _config.BigAsteroidMaxSpeed);
            asteroid.Unit.Data = data;
            asteroid.ForceUpdateTransform();
            asteroid.Unit.OnDied += BigAsteroidDiedHandler;
            _asteroids.Add(asteroid);
        }

        private void SpawnSmallAsteroids(Unit unit)
        {
            for (var i = 0; i < _config.ChildrenFromBigAsteroid; i++)
            {
                var asteroid = _smallAsteroidFactory.Get();
                var data = unit.Data;
                var deviationDegrees = Random.Range(-_config.DeviationDegrees, _config.DeviationDegrees);
                data.Speed = Vector2.right.Vector2FromAngle(360f / _config.ChildrenFromBigAsteroid * i + deviationDegrees)
                              * unit.Data.Speed.magnitude * _config.SmallAsteroidParentSpeedCoef;
                asteroid.Unit.Data = data;
                asteroid.ForceUpdateTransform();
                asteroid.Unit.OnDied += SmallAsteroidDiedHandler;
                _asteroids.Add(asteroid);
            }
        }

        private void BigAsteroidDiedHandler(Unit unit, bool real)
        {
            var index = _asteroids.FindIndex(x => x.Unit == unit);
            _asteroids[index].Unit.OnDied -= BigAsteroidDiedHandler;
            _bigAsteroidFactory.Release(_asteroids[index]);
            _asteroids.RemoveAt(index);

            if (real)
            {
                SpawnSmallAsteroids(unit);
                OnBigAsteroidDied?.Invoke();
            }
        }

        private void SmallAsteroidDiedHandler(Unit unit, bool real)
        {
            var index = _asteroids.FindIndex(x => x.Unit == unit);
            _asteroids[index].Unit.OnDied -= SmallAsteroidDiedHandler;
            _smallAsteroidFactory.Release(_asteroids[index]);
            _asteroids.RemoveAt(index);

            if (real)
            {
                OnSmallAsteroidDied?.Invoke();
            }
        }

        [Serializable]
        public class Config
        {
            [Min(0)] public float TimeForSpawn;
            public float AccumulatedTime;

            [Header("Big Asteroid")]
            public float BigAsteroidMinSpeed;
            public float BigAsteroidMaxSpeed;
            public float ChildrenFromBigAsteroid;

            [Header("Small Asteroid")]
            public float DeviationDegrees;
            public float SmallAsteroidParentSpeedCoef;
        }
    }
}