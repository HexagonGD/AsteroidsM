using Asteroids.Logic.Common.Units.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Logic.Common.Services
{
    public class CompositeUnitRepository
    {
        public event Action<CompositeUnit> OnUnitRegistered;
        public event Action<CompositeUnit> OnUnitUnregistered;

        private readonly List<CompositeUnit> _units = new();
        public IEnumerable<CompositeUnit> Units => _units.Append(Ship);

        public CompositeUnit Ship { get; set; }

        public void Register(CompositeUnit unit)
        {
            _units.Add(unit);
            OnUnitRegistered?.Invoke(unit);
        }

        public void Unregister(CompositeUnit unit)
        {
            _units.Remove(unit);
            OnUnitUnregistered?.Invoke(unit);
        }
    }
}