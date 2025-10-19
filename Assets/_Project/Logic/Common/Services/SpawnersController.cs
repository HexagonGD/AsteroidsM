using Asteroids.Logic.Common.Spawners.Core;
using Asteroids.Logic.Common.Units.Core;
using System;
using System.Collections.Generic;

namespace Asteroids.Logic.Common.Services
{
    public class SpawnersController
    {
        public event Action<Unit> OnSpawnedUnit;

        private readonly IEnumerable<ISpawner<CompositeUnit>> _spawners;

        public SpawnersController(IEnumerable<ISpawner<CompositeUnit>> spawners)
        {
            _spawners = spawners;
            foreach (var spawner in _spawners)
                spawner.OnSpawned += SpawnedHandler;
        }

        public void Update(float deltaTime)
        {
            foreach (var spawner in _spawners)
            {
                spawner.Update(deltaTime);
            }
        }

        public void Clear()
        {
            foreach (var x in _spawners)
            {
                x.Clear();
            }
        }

        private void SpawnedHandler(CompositeUnit unit)
        {
            OnSpawnedUnit?.Invoke(unit.Unit);
        }
    }
}