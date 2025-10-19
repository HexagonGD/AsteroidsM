using Asteroids.Logic.Common.DieHandler.Core;
using Asteroids.Logic.Common.Spawners.Implementation;
using Asteroids.Logic.Common.Units.Core;

namespace Asteroids.Logic.Common.DieHandler.Implementation
{
    public class SpawnChildrenDieEffect : IDieHandler
    {
        private readonly SmallAsteroidSpawner _spawner;

        public SpawnChildrenDieEffect(SmallAsteroidSpawner spawner)
        {
            _spawner = spawner;
        }

        public void Handle(Unit unit)
        {
            _spawner.SpawnChildren(unit);
        }
    }
}