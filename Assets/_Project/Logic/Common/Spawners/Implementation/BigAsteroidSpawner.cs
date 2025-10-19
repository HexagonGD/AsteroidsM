using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Units.Core;
using Asteroids.Logic.Zones;
using UnityEngine;

namespace Asteroids.Logic.Common.Spawners.Implementation
{
    public partial class BigAsteroidSpawner : SimpleSpawner
    {
        private readonly PlayZone _playZone;
        private readonly ChangePositionOnOutsidePlayZone _spawnPosition;
        private readonly Config _config;

        public BigAsteroidSpawner(PlayZone playZone, CompositeUnitRepository unitRepository,
                                  ChangePositionOnOutsidePlayZone spawnPosition, Config config, Unit.Factory unitFactory, UnitView.Factory unitViewFactory) :
                             base(unitRepository, unitFactory, unitViewFactory, config.TimeForSpawn, config.AccumulatedTime)
        {
            _playZone = playZone;
            _spawnPosition = spawnPosition;
            _config = config;
        }

        protected override TransformData SetupTransform(TransformData data)
        {
            data = _spawnPosition.SetRandomPosition(data);
            var targetPosition = Vector2.zero;
            targetPosition.x = _playZone.Width * Random.Range(-0.25f, 0.25f);
            targetPosition.y = _playZone.Height * Random.Range(-0.25f, 0.25f);
            data.Speed = (targetPosition - data.Position).normalized * _config.Speed;
            return data;
        }
    }
}