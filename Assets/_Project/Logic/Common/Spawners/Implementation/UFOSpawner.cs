using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Units.Core;
using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public partial class UFOSpawner : SimpleSpawner
    {
        private readonly ChangePositionOnOutsidePlayZone _spawnPosition;
        private readonly Config _config;

        public UFOSpawner(ChangePositionOnOutsidePlayZone spawnPosition, CompositeUnitRepository unitRepository,
                          Config config, Unit.Factory unitFactory, UnitView.Factory unitViewFactory) :
                     base(unitRepository, unitFactory, unitViewFactory, config.TimeForSpawn, config.AccumulatedTime)
        {
            _spawnPosition = spawnPosition;
            _config = config;
        }

        protected override TransformData SetupTransform(TransformData data)
        {
            data = _spawnPosition.SetRandomPosition(data);
            data.Speed = Vector2.one * _config.Speed;
            return data;
        }
    }
}