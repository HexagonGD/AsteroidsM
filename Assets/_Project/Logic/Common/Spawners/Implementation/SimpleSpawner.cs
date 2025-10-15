using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services.Factory.Implementation;
using Asteroids.Logic.Common.Units;
using System;
using System.Collections.Generic;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public class SimpleSpawner : LoopTimerSpawner<CompositeUnit>
    {
        private readonly CompositeFactory _factory;
        private readonly Func<TransformData> _setupTransformData;
        protected readonly List<CompositeUnit> _units = new();

        public SimpleSpawner(CompositeFactory factory, Func<TransformData> setupTransformData, float timeForSpawn, float accumulatedTime) :
                             base(timeForSpawn, accumulatedTime)
        {
            _factory = factory;
            _setupTransformData = setupTransformData;
        }

        public sealed override void Clear()
        {
            _units.ForEach(x =>
            {
                x.Unit.OnDied -= DiedHandler;
                _factory.Release(x);
            });

            _units.Clear();
            base.Clear();
        }

        protected sealed override CompositeUnit SpawnHandler()
        {
            var unit = _factory.Get();
            unit.Unit.Data = _setupTransformData();
            unit.ForceUpdateTransform();
            unit.Unit.OnDied += DiedHandler;
            _units.Add(unit);
            return unit;
        }

        private void DiedHandler(Unit unit, bool real)
        {
            unit.OnDied -= DiedHandler;
            var index = _units.FindIndex(x => x.Unit == unit);
            _factory.Release(_units[index]);
            _units.RemoveAt(index);
        }
    }
}