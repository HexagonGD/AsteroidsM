using Asteroids.Factory.Interface;
using Asteroids.View;
using UnityEngine;

namespace Asteroids.Factory.Implementation
{
    public class UnitViewFactory : IFactory<UnitView>
    {
        private readonly UnitView _unitViewPrefab;

        public UnitViewFactory(UnitView unitViewPrefab)
        {
            _unitViewPrefab = unitViewPrefab;
        }

        public UnitView Get()
        {
            return Object.Instantiate(_unitViewPrefab);
        }
    }
}