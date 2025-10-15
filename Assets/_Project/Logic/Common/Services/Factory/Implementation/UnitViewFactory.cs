using Asteroids.Logic.Common.Services.Factory.Core;
using Asteroids.Logic.Common.Units;
using UnityEngine;

namespace Asteroids.Logic.Common.Services.Factory.Implementation
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