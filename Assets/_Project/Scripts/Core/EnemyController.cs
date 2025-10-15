using Asteroids.Spawners.Core;
using Asteroids.Spawners.Implementation;
using System;
using System.Collections.Generic;

namespace Asteroids.Core
{
    public class EnemyController
    {
        public event Action OnEnemyDied;

        private readonly List<ISpawner<CompositeUnit>> _spawners;

        private List<CompositeUnit> _units = new();

        public EnemyController(List<ISpawner<CompositeUnit>> spawners)
        {
            _spawners = spawners;
            _spawners.ForEach(x => x.OnSpawned += SpawnedHandler);
        }

        public void Update(float deltaTime)
        {
            _spawners.ForEach(x => x.Update(deltaTime));

            for (var i = _units.Count - 1; i >= 0; i--)
                _units[i].Update(deltaTime);
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