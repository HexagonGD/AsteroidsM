using System;

namespace Asteroids.Timers.Implementation
{
    public class LoopTimer
    {
        public event Action OnLoop;

        public float LoopTime { get; private set; }
        public float AccumulatedTime { get; private set; }

        public LoopTimer(float loopTime, float accumulatedTime = 0)
        {
            LoopTime = loopTime;
            AccumulatedTime = accumulatedTime;
        }

        public void Update(float deltaTime)
        {
            AccumulatedTime += deltaTime;

            while (AccumulatedTime >= LoopTime)
            {
                AccumulatedTime -= LoopTime;
                OnLoop?.Invoke();
            }
        }
    }
}