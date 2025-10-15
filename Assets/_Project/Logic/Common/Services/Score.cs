using R3;

namespace Asteroids.Logic.Common.Services
{
    public class Score
    {
        public ReactiveProperty<int> Value = new ReactiveProperty<int>(0);
    }
}