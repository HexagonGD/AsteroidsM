using Asteroids.Logic.Common.Services.Factory.Implementation;
using Asteroids.Logic.Common.Spawners.Core;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public class DiedParentChildrenSpawner : ISpawner<CompositeUnit>
    {
        public event Action<CompositeUnit> OnSpawned;

        private readonly CompositeFactory _factory;
        private readonly ISpawner<CompositeUnit> _parentSpawner;
        private readonly Config _config;
        private readonly List<CompositeUnit> _parents = new();
        private readonly List<CompositeUnit> _units = new();

        public DiedParentChildrenSpawner(CompositeFactory factory, ISpawner<CompositeUnit> spawner, Config config)
        {
            _factory = factory;
            _parentSpawner = spawner;
            _config = config;

            _parentSpawner.OnSpawned += ParentSpawnedHandler;
        }

        public void Update(float deltaTime) { }

        public void Clear()
        {
            _parents.ForEach(x => x.Unit.OnDied -= ParentDiedHandler);
            _units.ForEach(x =>
            {
                x.Unit.OnDied -= DiedHandler;
                _factory.Release(x);
            });

            _parents.Clear();
            _units.Clear();
        }

        private void ParentSpawnedHandler(CompositeUnit unit)
        {
            unit.Unit.OnDied += ParentDiedHandler;
            _parents.Add(unit);
        }

        private void ParentDiedHandler(Unit unit, bool real)
        {
            if (real)
            {
                for (var i = 0; i < _config.CountChildren; i++)
                {
                    var child = _factory.Get();
                    var data = unit.Data;
                    var deviationDegrees = UnityEngine.Random.Range(-_config.DeviationDegrees, _config.DeviationDegrees);
                    data.Speed = Vector2.right.Vector2FromAngle(360f / _config.CountChildren * i + deviationDegrees)
                                  * unit.Data.Speed.magnitude * _config.SpeedCoef;
                    child.Unit.Data = data;
                    child.ForceUpdateTransform();
                    child.Unit.OnDied += DiedHandler;
                    _units.Add(child);
                    OnSpawned(child);
                }
            }

            unit.OnDied -= ParentDiedHandler;
            var index = _parents.FindIndex(x => x.Unit == unit);
            _parents.RemoveAt(index);
        }

        private void DiedHandler(Unit unit, bool real)
        {
            unit.OnDied -= DiedHandler;
            var index = _units.FindIndex(x => x.Unit == unit);
            _factory.Release(_units[index]);
            _units.RemoveAt(index);
        }

        [System.Serializable]
        public class Config
        {
            public int CountChildren;
            public float SpeedCoef;
            public float DeviationDegrees;
        }
    }
}