using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Services.Factory.Implementation;
using Asteroids.Logic.Common.Spawners.Core;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Units.Core;
using Asteroids.Logic.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public partial class SmallAsteroidSpawner : ISpawner<CompositeUnit>
    {
        public event Action<CompositeUnit> OnSpawned;

        private readonly CompositeUnitRepository _unitRepository;
        private readonly CompositeFactory _factory;
        private readonly Config _config;
        private readonly List<CompositeUnit> _units = new();

        public SmallAsteroidSpawner(CompositeUnitRepository unitRepository, Unit.Factory unitFactory, UnitView.Factory unitViewFactory, Config config)
        {
            _unitRepository = unitRepository;
            _factory = new CompositeFactory(unitFactory, unitViewFactory);
            _config = config;
        }

        public void Update(float deltaTime) { }

        public void Clear()
        {
            _units.ForEach(x =>
            {
                x.Unit.OnDied -= DiedHandler;
                x.Unit.Die(false);
                _unitRepository.Unregister(x);
                _factory.Release(x);
            });

            _units.Clear();
        }

        public void SpawnChildren(Unit unit)
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
                _unitRepository.Register(child);
                OnSpawned(child);
            }
        }

        private void DiedHandler(Unit unit, bool real)
        {
            unit.OnDied -= DiedHandler;
            var index = _units.FindIndex(x => x.Unit == unit);
            _factory.Release(_units[index]);
            _unitRepository.Unregister(_units[index]);
            _units.RemoveAt(index);
        }
    }
}