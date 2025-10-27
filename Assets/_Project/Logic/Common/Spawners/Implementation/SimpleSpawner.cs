using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Services.Factory.Implementation;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Units.Core;
using System;
using System.Collections.Generic;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public abstract class SimpleSpawner : LoopTimerSpawner<CompositeUnit>
    {
        private readonly CompositeUnitRepository _unitRepository;
        private readonly CompositeFactory _factory;
        protected readonly List<CompositeUnit> _units = new();

        public SimpleSpawner(CompositeUnitRepository unitRepository, Unit.Factory unitFactory, UnitView.Factory unitViewFactory, float timeForSpawn, float accumulatedTime) :
                             base(timeForSpawn, accumulatedTime)
        {
            _unitRepository = unitRepository;
            _factory = new CompositeFactory(unitFactory, unitViewFactory);
        }

        public sealed override void Clear()
        {
            _units.ForEach(x =>
            {
                x.Unit.OnDied -= DiedHandler;
                x.Unit.Die(false);
                _unitRepository.Unregister(x);
                _factory.Release(x);
            });

            _units.Clear();
            base.Clear();
        }

        protected sealed override CompositeUnit SpawnHandler()
        {
            var unit = _factory.Get();
            unit.Unit.Data = SetupTransform(unit.Unit.Data);
            unit.ForceUpdateTransform();
            unit.Unit.OnDied += DiedHandler;
            _units.Add(unit);
            _unitRepository.Register(unit);
            return unit;
        }

        protected abstract TransformData SetupTransform(TransformData data);

        private void DiedHandler(Unit unit, bool real)
        {
            unit.OnDied -= DiedHandler;
            var index = _units.FindIndex(x => x.Unit == unit);
            _factory.Release(_units[index]);
            _unitRepository.Unregister(_units[index]);
            _units.RemoveAt(index);
        }

        public override void Dispose()
        {
            base.Dispose();
            Clear();
        }
    }
}