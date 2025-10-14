using R3;

namespace Asteroids.Core
{
    public class Score
    {
        public ReactiveProperty<int> Value = new ReactiveProperty<int>(0);
    }
}