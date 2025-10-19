using Asteroids.Logic.Common.Services.Factory.Core;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Units.Core;
using System.Collections.Generic;

namespace Asteroids.Logic.Common.Services.Factory.Implementation
{
    public class CompositeFactory : IPoolFactory<CompositeUnit>
    {
        private readonly Unit.Factory _unitFactory;
        private readonly UnitView.Factory _unitViewFactory;

        private Stack<CompositeUnit> _pool = new();

        public CompositeFactory(Unit.Factory unitFactory, UnitView.Factory unitViewFactory)
        {
            _unitFactory = unitFactory;
            _unitViewFactory = unitViewFactory;
        }

        public CompositeUnit Get()
        {
            if (_pool.TryPop(out var result) == false)
            {
                var unit = _unitFactory.Create();
                var unitView = _unitViewFactory.Create();
                result = new CompositeUnit(unit, unitView);
            }

            result.UnitView.Show();
            return result;
        }

        public void Release(CompositeUnit value)
        {
            value.UnitView.Hide();
            _pool.Push(value);
        }
    }
}