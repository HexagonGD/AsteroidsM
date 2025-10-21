using UnityEngine;

namespace Asteroids.Logic.Analytics.Implementation
{
    public static class EventNames
    {
        public const string BulletWeaponFiredEvent = "bulletWeaponFired";
        public const string LazerWeaponFiredEvent = "lazerWeaponFired";

        public const string UFODiedEvent = "ufoDied";
        public const string SmallAsteroidDiedEvent = "smallAsteroidDied";
        public const string BigAsteroidDiedEvent = "bigAsteroidDied";
        public const string AnyAsteroidDiedEvent = "asteroidDied";


        public const string StartGameEvent = "startGame";
        public const string EndGameEvent = "endGame";
    }
}