using UnityEngine;

namespace Asteroids.Core
{
    public class SpawnOutsideGameZone
    {
        public Vector2 GetSpawnPosition(PlayZone playZone)
        {
            var x = Random.Range(-playZone.Width / 2f, playZone.Width / 2f);
            var y = Random.Range(-playZone.Height / 2f, playZone.Height / 2f);

            x += x < 0 ? -playZone.Width * .5f : playZone.Width * .5f;
            y += y < 0 ? -playZone.Height * .5f : playZone.Height * .5f;

            return new Vector2(x, y);
        }
    }
}