using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units.Implementation;
using Asteroids.Logic.Common.Weapon.Implementation;
using R3;
using System;
using UnityEngine;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class DebugViewModel : IDisposable
    {
        public ReadOnlyReactiveProperty<int> Score { get; private set; }
        public ReadOnlyReactiveProperty<Vector2> Position { get; private set; }
        public ReadOnlyReactiveProperty<float> Rotation { get; private set; }
        public ReadOnlyReactiveProperty<int> LazerCharges { get; private set; }
        public ReadOnlyReactiveProperty<float> ChargeReload { get; private set; }

        private IDisposable _disposable;

        public DebugViewModel(Score score, Ship ship, LazerWeapon lazerWeapon)
        {
            Score = score.Value.ToReadOnlyReactiveProperty();
            Position = Observable.EveryValueChanged(ship, x => x.Data.Position).ToReadOnlyReactiveProperty<Vector2>();
            Rotation = Observable.EveryValueChanged(ship, x => x.Data.Rotation).ToReadOnlyReactiveProperty<float>();
            LazerCharges = Observable.EveryValueChanged(lazerWeapon, x => x.Charges).ToReadOnlyReactiveProperty<int>();
            ChargeReload = Observable.EveryValueChanged(lazerWeapon, x => x.RemainingTimeForCharge).ToReadOnlyReactiveProperty<float>();

            _disposable = Disposable.Combine(Score, Position, Rotation, LazerCharges, ChargeReload);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}