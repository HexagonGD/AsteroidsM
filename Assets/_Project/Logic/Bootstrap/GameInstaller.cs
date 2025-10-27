using UnityEngine;
using Zenject;
using Asteroids.Logic.Common.Movement.Implementation;
using Asteroids.Logic.Common.Weapon.Implementation;
using Asteroids.Logic.Common.Weapon.View;
using Asteroids.Logic.Common.UI.Implementation;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Spawners.Implementation;
using Asteroids.Logic.Zones;
using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Weapon.Core;
using Asteroids.Logic.Common.UI.Core;
using Asteroids.Logic.FSMachine;
using Asteroids.Logic.Common.Movement.DeadZoneHandler.Implementation;
using Asteroids.Logic.Common.DieHandler.Implementation;
using Asteroids.Logic.Common.Movement.BorderHandler.Implementation;
using Asteroids.Logic.Movement.BorderHandler.Implementation;
using Asteroids.Logic.Common.Spawners.Core;
using Asteroids.Logic.Content;
using Asteroids.Logic.Common.Units.Implementation;
using Asteroids.Logic.Common.Units.Core;
using Asteroids.Logic.Common.Services.Saving.Core;
using Asteroids.Logic.Common.Services.Saving.Implementation;
using Asteroids.Logic.Analytics.Core;
using Asteroids.Logic.Analytics.Implementation.UnitDiedListeners;
using Asteroids.Logic.Analytics.Implementation.WeaponListeners;
using Asteroids.Logic.Analytics.Implementation;

namespace Asteroids.Logic.Bootstrap
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private AccelerationMovementConfig _shipMovementConfig;

        [Header("Weapon Configs")]
        [SerializeField] private BulletWeaponConfig _bulletWeaponConfig;
        [SerializeField] private LazerWeaponConfig _lazerWeaponConfig;
        [SerializeField] private LazerRendererConfig _lazerRendererConfig;

        [Header("Spawner Configs")]
        [SerializeField] private UFOSpawnerConfig _ufoSpawnerConfig;
        [SerializeField] private BigAsteroidSpawnerConfig _bigAsteroidSpawnerConfig;
        [SerializeField] private SmallAsteroidSpawnerConfig _smallAsteroidSpawnerConfig;

        [Header("Prefabs")]
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private LineRenderer _lazerPrefab;

        private LocalPrefabLoader _loader = new();

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayZone>().AsSingle().WithArguments(_camera);
            Container.BindInterfacesAndSelfTo<SpawnersController>().AsSingle();
            Container.BindInterfacesAndSelfTo<Arsenal>().AsSingle();
            Container.BindInterfacesAndSelfTo<LazerRendererController>().AsSingle();
            Container.BindInterfacesAndSelfTo<FSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<Score>().AsSingle();
            Container.Bind<ISaveManager>().To<PlayerPrefsSaveManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<FinalScoreViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugView>().FromComponentInNewPrefab(_loader.LoadInternal<DebugView>(_prefabsConfig.DebugViewPrefab)).UnderTransform(_canvas.transform).AsSingle();
            Container.BindInterfacesAndSelfTo<FinalScoreView>().FromComponentInNewPrefab(_loader.LoadInternal<FinalScoreView>(_prefabsConfig.FinalScoreViewPrefab)).UnderTransform(_canvas.transform).AsSingle();

            Container.Bind<IWeapon>().WithId("first").To<BulletWeapon>().FromResolve().AsCached();
            Container.Bind<IWeapon>().WithId("second").To<LazerWeapon>().FromResolve().AsCached();
            Container.BindInterfacesAndSelfTo<BulletWeapon>().AsCached();
            Container.BindInterfacesAndSelfTo<LazerWeapon>().AsCached();

            Container.BindInstances(_bulletWeaponConfig, _lazerWeaponConfig, _lazerPrefab, _lazerRendererConfig, _shipMovementConfig,
                                    _ufoSpawnerConfig, _smallAsteroidSpawnerConfig, _bigAsteroidSpawnerConfig);
            Container.QueueForInject(_bulletWeaponConfig);
            Container.QueueForInject(_lazerWeaponConfig);
            Container.QueueForInject(_lazerPrefab);
            Container.QueueForInject(_lazerRendererConfig);
            Container.QueueForInject(_shipMovementConfig);
            Container.QueueForInject(_ufoSpawnerConfig);
            Container.QueueForInject(_smallAsteroidSpawnerConfig);
            Container.QueueForInject(_bigAsteroidSpawnerConfig);

            Container.BindInterfacesAndSelfTo<ChangePositionOnOutsidePlayZone>().AsSingle().WithArguments<float>(3f);
            Container.BindInterfacesAndSelfTo<CompositeUnitRepository>().AsSingle();

            Container.BindInterfacesAndSelfTo<AccelerationMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<LinearMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveToShip>().AsSingle();

            Container.BindInterfacesAndSelfTo<IgnoreBorder>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScreenBorderTeleport>().AsSingle();

            Container.BindInterfacesAndSelfTo<IngoreDeadZone>().AsSingle();
            Container.BindInterfacesAndSelfTo<OffsetBorderDeadZone>().AsSingle().WithArguments<float>(3f);

            Container.BindInterfacesAndSelfTo<WithoutDieEffect>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnChildrenDieEffect>().AsSingle();

            Container.Bind<ISpawner<CompositeUnit>>().To<UFOSpawner>().AsCached();
            Container.Bind<ISpawner<CompositeUnit>>().To<SmallAsteroidSpawner>().FromResolve().AsCached();
            Container.BindInterfacesAndSelfTo<SmallAsteroidSpawner>().AsCached();
            Container.Bind<ISpawner<CompositeUnit>>().To<BigAsteroidSpawner>().AsCached();

            Container.BindInterfacesAndSelfTo<Ship>().AsSingle();
            Container.BindFactory<Unit, Unit.Factory>().To<Bullet>().WhenInjectedInto<BulletWeapon>();
            Container.BindFactory<Unit, Unit.Factory>().To<UFO>().WhenInjectedInto<UFOSpawner>();
            Container.BindFactory<Unit, Unit.Factory>().To<SmallAsteroid>().WhenInjectedInto<SmallAsteroidSpawner>();
            Container.BindFactory<Unit, Unit.Factory>().To<BigAsteroid>().WhenInjectedInto<BigAsteroidSpawner>();

            Container.BindInterfacesAndSelfTo<UnitView>().FromComponentInNewPrefab(_loader.LoadInternal<UnitView>(_prefabsConfig.ShipPrefab)).AsSingle().WhenInjectedInto<Game>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_loader.LoadInternal<UnitView>(_prefabsConfig.BulletPrefab)).UnderTransformGroup("Bullets").WhenInjectedInto<BulletWeapon>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_loader.LoadInternal<UnitView>(_prefabsConfig.UFOPrefab)).UnderTransformGroup("UFO").WhenInjectedInto<UFOSpawner>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_loader.LoadInternal<UnitView>(_prefabsConfig.SmallAsteroidPrefab)).UnderTransformGroup("SmallAsteroids").WhenInjectedInto<SmallAsteroidSpawner>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_loader.LoadInternal<UnitView>(_prefabsConfig.BigAsteroidPrefab)).UnderTransformGroup("BigAsteroids").WhenInjectedInto<BigAsteroidSpawner>();

            Container.BindInterfacesAndSelfTo<UISwitcher>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Game>().AsSingle().NonLazy();

            BindAnalytic();
        }

        private void BindAnalytic()
        {
            Container.BindInterfacesAndSelfTo<UFODiedAnalyticListener>().AsSingle();
            Container.BindInterfacesAndSelfTo<SmallAsteroidDiedAnalyticListener>().AsSingle();
            Container.BindInterfacesAndSelfTo<BigAsteroidDiedAnalyticListener>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsteroidDiedAnalyticListener>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletWeaponFireAnalyticListener>().AsSingle();
            Container.Bind<IAnalyticListener>().To<LazerWeaponFireAnalyticListener>().FromResolve().AsCached();
            Container.BindInterfacesAndSelfTo<LazerWeaponFireAnalyticListener>().AsCached();
            Container.Bind<IAnalyticListener>().To<StartGameAnalyticListener>().AsSingle();
            Container.Bind<IAnalyticListener>().To<EndGameAnalyticListener>().AsSingle();
            Container.Bind<IAnalytic>().To<FirebaseAdapter>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnalyticManager>().AsSingle().NonLazy();
        }

        private void OnDestroy()
        {
            _loader.ReleaseAll();
        }
    }
}