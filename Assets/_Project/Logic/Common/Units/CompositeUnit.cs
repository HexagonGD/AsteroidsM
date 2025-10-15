using Asteroids.Logic.Common.Movement.Core;

namespace Asteroids.Logic.Common.Units
{
    public class CompositeUnit
    {
        public readonly Unit Unit;
        public readonly UnitView UnitView;

        private TransformAdapter _transformAdapter = new TransformAdapter();

        public CompositeUnit(Unit unit, UnitView unitView)
        {
            Unit = unit;
            UnitView = unitView;
        }

        public void Update(float deltaTime)
        {
            Unit.Update(deltaTime);
            _transformAdapter.Update(UnitView.transform, Unit.Data);
        }

        public void ForceUpdateTransform()
        {
            _transformAdapter.Update(UnitView.transform, Unit.Data);
        }
    }
}