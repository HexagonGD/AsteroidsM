using System;
using UnityEngine;

public class ShipSystem
{
    public event Action ShipDeadEvent;

    private Arsenal _arsenal;
    private CompositeFactory _shipFactory;
    private CompositeFactory _bulletFactory;
    private Config _config;

    private CompositeUnit _ship;
    private LazerWeapon _lazerWeapon;

    public Unit ShipUnit => _ship.Unit;
    public int LazerCharges => _lazerWeapon.Charges;
    public float LazerRemainingTime => _lazerWeapon.RemainingTimeForCharge;

    public ShipSystem(Config config, PlayZone playZone)
    {
        _config = config;

        var movement = new WASDMovement(_config.MovementConfig);
        var borderHandler = new ScreenBorderTeleport(playZone);
        var dieEffect = new InvokeActionDieEffect(OnShipDead);

        var shipUnitFactory = new UnitFactory(movement, borderHandler, dieEffect);
        var bulletUnitFactory = new UnitFactory(new LinearMovement(), new IgnoreBorder(), new WithoutDieEffect());

        var unitViewFactory = new UnitViewFactory(_config.ShipPrefab);
        var bulletUnitViewFactory = new UnitViewFactory(_config.BulletPrefab);

        _shipFactory = new CompositeFactory(shipUnitFactory, unitViewFactory);
        _bulletFactory = new CompositeFactory(bulletUnitFactory, bulletUnitViewFactory);

        _ship = _shipFactory.Get();

        var data = new TransformData(Vector2.zero, Vector2.zero, 0);
        _ship.Unit.Data = data;

        LazerRendererController lazerRendererCtrl = new(_config.LineRendererPrefab, _config.LazerRendererConfig);

        var firstWeapon = new BulletWeapon(_ship.Unit, _bulletFactory, _config.BulletWeaponConfig, playZone);
        _lazerWeapon = new LazerWeapon(_ship.Unit, lazerRendererCtrl, _config.LazerWeaponConfig);

        _arsenal = new Arsenal(firstWeapon, _lazerWeapon);
    }

    public void Update(float deltaTime)
    {
        _ship.Update(deltaTime);
        _arsenal.Update(deltaTime);
    }

    public void Clear()
    {
        _ship.Unit.Data = new TransformData();
        _arsenal.Clear();
    }

    private void OnShipDead(Unit unit)
    {
        ShipDeadEvent?.Invoke();
    }

    [System.Serializable]
    public class Config
    {
        public WASDMovement.SpeedData MovementConfig;
        public UnitView ShipPrefab;
        public UnitView BulletPrefab;

        public LineRenderer LineRendererPrefab;
        public LazerRenderer.Config LazerRendererConfig;

        public BulletWeapon.Config BulletWeaponConfig;
        public LazerWeapon.Config LazerWeaponConfig;
    }
}