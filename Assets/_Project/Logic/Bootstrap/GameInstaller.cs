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
using Asteroids.Logic.Remote;
using Asteroids.Logic.Common.Configs.Implementation;

namespace Asteroids.Logic.Bootstrap
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;

        [Header("Weapon Configs")]
        [SerializeField] private LazerRendererConfig _lazerRendererConfig;

        [Header("Prefabs")]
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private LineRenderer _lazerPrefab;

        private LocalPrefabLoader[] _loaders;

        public override void InstallBindings()
        {
            var debugViewLoader = new LocalPrefabLoader();
            var finalScoreViewLoader = new LocalPrefabLoader();
            var shipLoader = new LocalPrefabLoader();
            var bulletLoader = new LocalPrefabLoader();
            var ufoLoader = new LocalPrefabLoader();
            var smallAsteroidLoader = new LocalPrefabLoader();
            var bigAsteroidLoader = new LocalPrefabLoader();

            _loaders = new[] { debugViewLoader, finalScoreViewLoader, shipLoader, bulletLoader, ufoLoader, smallAsteroidLoader, bigAsteroidLoader };

            Container.Bind<PlayZone>().AsSingle().WithArguments(_camera);
            Container.Bind<SpawnersController>().AsSingle();
            Container.Bind<Arsenal>().AsSingle();
            Container.Bind<LazerRendererController>().AsSingle();
            Container.Bind<FSM>().AsSingle();
            Container.Bind<Score>().AsSingle();
            Container.Bind<ISaveManager>().To<PlayerPrefsSaveManager>().AsSingle();
            Container.Bind<DebugViewModel>().AsSingle();
            Container.Bind<FinalScoreViewModel>().AsSingle();
            var debugView = debugViewLoader.LoadInternal<DebugView>(_prefabsConfig.DebugViewPrefab);
            Container.Bind<DebugView>().FromComponentInNewPrefab(debugView).UnderTransform(_canvas.transform).AsSingle();
            Container.Bind<FinalScoreView>().FromComponentInNewPrefab(finalScoreViewLoader.LoadInternal<FinalScoreView>(_prefabsConfig.FinalScoreViewPrefab)).UnderTransform(_canvas.transform).AsSingle();

            Container.Bind<IWeapon>().WithId("first").To<BulletWeapon>().FromResolve().AsCached();
            Container.Bind<IWeapon>().WithId("second").To<LazerWeapon>().FromResolve().AsCached();
            Container.Bind<BulletWeapon>().AsCached();
            Container.Bind<LazerWeapon>().AsCached();

            Container.BindInstances(_lazerPrefab, _lazerRendererConfig);
            Container.QueueForInject(_lazerPrefab);
            Container.QueueForInject(_lazerRendererConfig);

            Container.Bind<ChangePositionOnOutsidePlayZone>().AsSingle().WithArguments<float>(3f);
            Container.Bind<CompositeUnitRepository>().AsSingle();

            Container.Bind<AccelerationMovement>().AsSingle();
            Container.Bind<LinearMovement>().AsSingle();
            Container.Bind<MoveToShip>().AsSingle();

            Container.Bind<IgnoreBorder>().AsSingle();
            Container.Bind<ScreenBorderTeleport>().AsSingle();

            Container.Bind<IngoreDeadZone>().AsSingle();
            Container.Bind<OffsetBorderDeadZone>().AsSingle().WithArguments<float>(3f);

            Container.Bind<WithoutDieEffect>().AsSingle();
            Container.Bind<SpawnChildrenDieEffect>().AsSingle();

            Container.Bind<SmallAsteroidSpawner>().AsCached();
            Container.Bind<ISpawner<CompositeUnit>>().To<SmallAsteroidSpawner>().FromResolve().AsCached();
            Container.Bind<ISpawner<CompositeUnit>>().To<BigAsteroidSpawner>().AsSingle();
            Container.Bind<ISpawner<CompositeUnit>>().To<UFOSpawner>().AsSingle();

            Container.Bind<Ship>().AsSingle();
            Container.BindFactory<Unit, Unit.Factory>().To<Bullet>().WhenInjectedInto<BulletWeapon>();
            Container.BindFactory<Unit, Unit.Factory>().To<UFO>().WhenInjectedInto<UFOSpawner>();
            Container.BindFactory<Unit, Unit.Factory>().To<SmallAsteroid>().WhenInjectedInto<SmallAsteroidSpawner>();
            Container.BindFactory<Unit, Unit.Factory>().To<BigAsteroid>().WhenInjectedInto<BigAsteroidSpawner>();

            Container.Bind<UnitView>().FromComponentInNewPrefab(shipLoader.LoadInternal<UnitView>(_prefabsConfig.ShipPrefab)).AsSingle().WhenInjectedInto<Game>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(bulletLoader.LoadInternal<UnitView>(_prefabsConfig.BulletPrefab)).UnderTransformGroup("Bullets").WhenInjectedInto<BulletWeapon>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(ufoLoader.LoadInternal<UnitView>(_prefabsConfig.UFOPrefab)).UnderTransformGroup("UFO").WhenInjectedInto<UFOSpawner>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(smallAsteroidLoader.LoadInternal<UnitView>(_prefabsConfig.SmallAsteroidPrefab)).UnderTransformGroup("SmallAsteroids").WhenInjectedInto<SmallAsteroidSpawner>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(bigAsteroidLoader.LoadInternal<UnitView>(_prefabsConfig.BigAsteroidPrefab)).UnderTransformGroup("BigAsteroids").WhenInjectedInto<BigAsteroidSpawner>();

            Container.Bind<UISwitcher>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Game>().AsSingle().NonLazy();

            BindConfigs();
            BindAnalytic();
        }

        private void BindConfigs()
        {
            Container.BindInterfacesAndSelfTo<RemoteConfigsLoader>().AsSingle().NonLazy();

            Container.Bind<BulletWeaponConfig>().AsCached();
            Container.Bind<LazerWeaponConfig>().AsCached();
            Container.Bind<SmallAsteroidSpawnerConfig>().AsCached();
            Container.Bind<BigAsteroidSpawnerConfig>().AsCached();
            Container.Bind<AccelerationMovementConfig>().AsCached();
            Container.Bind<UFOSpawnerConfig>().AsCached();

            Container.Bind<IRemoteConfig>().To<BulletWeaponConfig>().FromResolve().AsCached();
            Container.Bind<IRemoteConfig>().To<LazerWeaponConfig>().FromResolve().AsCached();
            Container.Bind<IRemoteConfig>().To<SmallAsteroidSpawnerConfig>().FromResolve().AsCached();
            Container.Bind<IRemoteConfig>().To<BigAsteroidSpawnerConfig>().FromResolve().AsCached();
            Container.Bind<IRemoteConfig>().To<AccelerationMovementConfig>().FromResolve().AsCached();
            Container.Bind<IRemoteConfig>().To<UFOSpawnerConfig>().FromResolve().AsCached();

            //Container.Bind<IRemoteConfig>().To<BulletWeaponConfig>().AsCached();
            //Container.Bind<IRemoteConfig>().To<LazerWeaponConfig>().AsCached();
            //Container.Bind<IRemoteConfig>().To<SmallAsteroidSpawnerConfig>().AsCached();
            //Container.Bind<IRemoteConfig>().To<BigAsteroidSpawnerConfig>().AsCached();
            //Container.Bind<IRemoteConfig>().To<AccelerationMovementConfig>().AsCached();
            //Container.Bind<IRemoteConfig>().To<UFOSpawnerConfig>().AsCached();
        }

        private void BindAnalytic()
        {
            Container.Bind<UFODiedAnalyticListener>().AsSingle();
            Container.Bind<SmallAsteroidDiedAnalyticListener>().AsSingle();
            Container.Bind<BigAsteroidDiedAnalyticListener>().AsSingle();
            Container.Bind<AsteroidDiedAnalyticListener>().AsSingle();
            Container.Bind<BulletWeaponFireAnalyticListener>().AsSingle();
            Container.Bind<IAnalyticListener>().To<LazerWeaponFireAnalyticListener>().FromResolve().AsCached();
            Container.Bind<LazerWeaponFireAnalyticListener>().AsCached();
            Container.Bind<IAnalyticListener>().To<StartGameAnalyticListener>().AsSingle();
            Container.Bind<IAnalyticListener>().To<EndGameAnalyticListener>().AsSingle();
            Container.Bind<IAnalytic>().To<FirebaseAdapter>().AsSingle();
            Container.Bind<AnalyticManager>().AsSingle().NonLazy();
        }

        private void OnDestroy()
        {
            foreach (var loader in _loaders)
                loader.ReleaseInternal();
        }
    }
}