namespace Asteroids.Logic.Common.Configs.Core
{
    public interface ILoopTimerSpawnerConfig
    {
        public float TimeForSpawn { get; }
        public float AccumulatedTime { get; }
    }
}