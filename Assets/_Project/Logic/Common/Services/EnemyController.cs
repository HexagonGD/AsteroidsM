using Asteroids.Logic.Common.Spawners.Core;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Zones;
using System;
using System.Collections.Generic;

namespace Asteroids.Logic.Common.Services
{
    public class EnemyController
    {
        public event Action OnEnemyDied;

        private readonly List<ISpawner<CompositeUnit>> _spawners;

        private List<CompositeUnit> _units = new();
        private PlayZone _playZone;
        private DeadZone _deadzone = new();

        public EnemyController(List<ISpawner<CompositeUnit>> spawners, PlayZone playZone)
        {
            _spawners = spawners;
            _playZone = playZone;
            _spawners.ForEach(x => x.OnSpawned += SpawnedHandler);
        }

        public void Update(float deltaTime)
        {
            _spawners.ForEach(x => x.Update(deltaTime));

            for (var i = _units.Count - 1; i >= 0; i--)
            {
                _units[i].Update(deltaTime);
                if (_deadzone.CheckInDeadZone(_units[i].Unit.Data, _playZone))
                    _units[i].Unit.Die(false);
            }
        }

        public void Clear()
        {
            _units.ForEach(x => x.Unit.OnDied -= DiedHandler);
            _units.Clear();

            _spawners.ForEach(x => x.Clear());
        }

        private void SpawnedHandler(CompositeUnit unit)
        {
            unit.Unit.OnDied += DiedHandler;
            _units.Add(unit);
        }

        private void DiedHandler(Unit unit, bool real)
        {
            if (real)
                OnEnemyDied?.Invoke();

            unit.OnDied -= DiedHandler;
            var index = _units.FindIndex(x => x.Unit == unit);
            _units.RemoveAt(index);
        }
    }
}