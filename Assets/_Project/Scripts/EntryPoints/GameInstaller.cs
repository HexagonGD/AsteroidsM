using Asteroids.Core;
using Asteroids.Core.BorderHandler.Implementation;
using Asteroids.Core.BorderHandler.Interface;
using Asteroids.Core.Movement.Implementation;
using Asteroids.Core.Movement.Interface;
using Asteroids.Core.Weapon;
using Asteroids.Core.Weapon.Implementation;
using Asteroids.Core.Weapon.Interface;
using Asteroids.Factory.Implementation;
using Asteroids.View;
using Asteroids.FSMachine;
using UnityEngine;
using Zenject;
using Asteroids.UI.Core;
using Asteroids.UI.Implementation;
using Asteroids.Spawners.Implementation;
using System;
using Random = UnityEngine.Random;
using Asteroids.Spawners.Core;
using System.Collections.Generic;

namespace Asteroids
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private AccelerationMovement.SpeedData _shipMovementConfig;
        [SerializeField] private MoveToTarget.Config _ufoMovementConfig;

        [Header("Weapon Configs")]
        [SerializeField] private BulletWeapon.Config _bulletWeaponConfig;
        [SerializeField] private LazerWeapon.Config _lazerWeaponConfig;
        [SerializeField] private LazerRenderer.Config _lazerRendererConfig;

        [Header("UI")]
        [SerializeField] private DebugView _debugView;
        [SerializeField] private FinalScoreView _finalScoreView;

        [Header("Prefabs")]
        [SerializeField] private UnitView _bulletPrefab;
        [SerializeField] private UnitView _shipPrefab;
        [SerializeField] private UnitView _smallAsteroidPrefab;
        [SerializeField] private UnitView _bigAsteroidPrefab;
        [SerializeField] private UnitView _ufoPrefab;
        [SerializeField] private LineRenderer _lazerPrefab;

        [Header("Spawners")]
        [SerializeField, Min(0)] private float _timeForSpawnBigAsteroid;
        [SerializeField] private float _accumulatedTimeBigAsteroid;
        [SerializeField, Min(0)] private float _minSpeedBigAsteroid;
        [SerializeField, Min(0)] private float _maxSpeedBigAsteroid;
        [SerializeField, Min(0)] private float _timeForSpawnUFO;
        [SerializeField] private float _accumulatedTimeUFO;
        [SerializeField] private DiedParentChildrenSpawner.Config _smallAsteroidSpawnerConfig;

        public override void InstallBindings()
        {
            var playZone = new PlayZone(_camera);

            Func<TransformData> bigAsteroidSetupData = () =>
            {
                var data = new TransformData();
                var spawnPosition = new SpawnOutsideGameZone();

                data.Position = spawnPosition.GetSpawnPosition(playZone);

                var x = Random.Range(-playZone.Width / 4f, playZone.Width / 4f);
                var y = Random.Range(-playZone.Height / 4f, playZone.Height / 4f);

                data.Speed = (new Vector2(x, y) - data.Position).normalized * Random.Range(_minSpeedBigAsteroid, _maxSpeedBigAsteroid);
                return data;
            };

            Func<TransformData> ufoSetupData = () =>
            {
                var data = new TransformData();
                var spawnPosition = new SpawnOutsideGameZone();
                data.Position = spawnPosition.GetSpawnPosition(playZone);
                return data;
            };

            var shipFactory = BuildCompositeFactory(new AccelerationMovement(_shipMovementConfig), new ScreenBorderTeleport(playZone), _shipPrefab);
            var ship = shipFactory.Get();
            
            var bulletFactory = BuildCompositeFactory(new LinearMovement(), new IgnoreBorder(), _bulletPrefab);
            var smallAsteroidFactory = BuildCompositeFactory(new LinearMovement(), new IgnoreBorder(), _smallAsteroidPrefab);
            var bigAsteroidFactory = BuildCompositeFactory(new LinearMovement(), new IgnoreBorder(), _bigAsteroidPrefab);
            var ufoFactory = BuildCompositeFactory(new MoveToTarget(ship.Unit, _ufoMovementConfig), new IgnoreBorder(), _ufoPrefab);

            var bigAsteroidSpawner = new SimpleSpawner(bigAsteroidFactory, bigAsteroidSetupData, _timeForSpawnBigAsteroid, _accumulatedTimeBigAsteroid);
            var ufoSpawner = new SimpleSpawner(ufoFactory, ufoSetupData, _timeForSpawnUFO, _accumulatedTimeUFO);
            var smallAsteroidSpawner = new DiedParentChildrenSpawner(smallAsteroidFactory, bigAsteroidSpawner, _smallAsteroidSpawnerConfig);
            var spawners = new List<ISpawner<CompositeUnit>>() { bigAsteroidSpawner, ufoSpawner, smallAsteroidSpawner };

            Container.Bind<List<ISpawner<CompositeUnit>>>().FromInstance(spawners).AsSingle();
            Container.QueueForInject(spawners);

            Container.Bind<EnemyController>().AsSingle();

            Container.Bind<CompositeFactory>().FromInstance(bulletFactory).AsSingle().WhenInjectedInto<BulletWeapon>();

            Container.Bind<CompositeUnit>().FromInstance(ship).AsSingle();
            Container.Bind<Unit>().FromInstance(ship.Unit).AsSingle();
            Container.Bind<PlayZone>().FromInstance(playZone).AsSingle();
            Container.Bind<Arsenal>().AsSingle();
            Container.Bind<SpawnOutsideGameZone>().AsSingle();
            Container.Bind<LazerRendererController>().AsSingle();
            Container.Bind<FSM>().AsSingle();
            Container.Bind<Score>().AsSingle();
            Container.Bind<DebugViewModel>().AsSingle();
            Container.Bind<FinalScoreViewModel>().AsSingle();

            Container.Bind<IWeapon>().WithId("first").To<BulletWeapon>().AsSingle();
            Container.Bind<IWeapon>().WithId("second").To<LazerWeapon>().FromResolve().AsCached();
            Container.Bind<LazerWeapon>().AsCached();

            Container.BindInstances(_bulletWeaponConfig, _lazerWeaponConfig,
                                    _lazerPrefab, _lazerRendererConfig, _debugView, _finalScoreView);
            Container.QueueForInject(_bulletWeaponConfig);
            Container.QueueForInject(_lazerWeaponConfig);
            Container.QueueForInject(_lazerPrefab);
            Container.QueueForInject(_lazerRendererConfig);
            Container.QueueForInject(_debugView);
            Container.QueueForInject(_finalScoreView);
            Container.QueueForInject(ship);
            Container.QueueForInject(ship.Unit);
            Container.QueueForInject(bulletFactory);
            Container.QueueForInject(smallAsteroidFactory);
            Container.QueueForInject(bigAsteroidFactory);
            Container.QueueForInject(ufoFactory);
            

            Container.Bind<UISwitcher>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Game>().AsSingle().NonLazy();
        }

        private CompositeFactory BuildCompositeFactory(IMovement movement, IBorderHandler borderHandler, UnitView prefab)
        {
            var unitFactory = new UnitFactory(movement, borderHandler);
            var unitViewFactory = new UnitViewFactory(prefab);
            return new CompositeFactory(unitFactory, unitViewFactory);
        }
    }
}