using Asteroids.Core;
using Asteroids.Factory.Interface;
using Asteroids.View;
using System.Collections.Generic;

namespace Asteroids.Factory.Implementation
{
    public class CompositeFactory : IPoolFactory<CompositeUnit>
    {
        private readonly IFactory<Unit> _unitFactory;
        private readonly IFactory<UnitView> _unitViewFactory;

        private Stack<CompositeUnit> _pool = new();

        public CompositeFactory(IFactory<Unit> unitFactory, IFactory<UnitView> unitViewFactory)
        {
            _unitFactory = unitFactory;
            _unitViewFactory = unitViewFactory;
        }

        public CompositeUnit Get()
        {
            if (_pool.TryPop(out var result) == false)
            {
                var unit = _unitFactory.Get();
                var unitView = _unitViewFactory.Get();
                unitView.Unit = unit;
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