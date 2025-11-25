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
using Asteroids.Logic.Analytics.Core;
using Asteroids.Logic.Analytics.Implementation.UnitDiedListeners;
using Asteroids.Logic.Analytics.Implementation.WeaponListeners;
using Asteroids.Logic.Analytics.Implementation;
using Asteroids.Logic.Remote;
using Asteroids.Logic.Common.Configs.Implementation;
using Asteroids.Logic.Ads.Implementation;

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
        [SerializeField] private UnityAdsProvider _adsProviderPrefab;
        [SerializeField] private SceneContext _sceneContext;

        private LocalPrefabLoader _loader = new();
        private DebugView _debugView;
        private FinalScoreView _finalScoreView;
        private RebirthView _rebirthView;
        private UnitView _bullet;
        private UnitView _smallAsteroid;
        private UnitView _bigAsteroid;
        private UnitView _ufo;
        private UnitView _ship;

        private async void Awake()
        {
            _debugView = await _loader.LoadInternal<DebugView>(_prefabsConfig.DebugViewPrefab);
            _finalScoreView = await _loader.LoadInternal<FinalScoreView>(_prefabsConfig.FinalScoreViewPrefab);
            _rebirthView = await _loader.LoadInternal<RebirthView>(_prefabsConfig.RebirthViewPrefab);
            _bullet = await _loader.LoadInternal<UnitView>(_prefabsConfig.BulletPrefab);
            _smallAsteroid = await _loader.LoadInternal<UnitView>(_prefabsConfig.SmallAsteroidPrefab);
            _bigAsteroid = await _loader.LoadInternal<UnitView>(_prefabsConfig.BigAsteroidPrefab);
            _ufo = await _loader.LoadInternal<UnitView>(_prefabsConfig.UFOPrefab);
            _ship = await _loader.LoadInternal<UnitView>(_prefabsConfig.ShipPrefab);
            _sceneContext.Run();
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayZone>().AsSingle().WithArguments(_camera);
            Container.BindInterfacesAndSelfTo<SpawnersController>().AsSingle();
            Container.BindInterfacesAndSelfTo<Arsenal>().AsSingle();
            Container.BindInterfacesAndSelfTo<LazerRendererController>().AsSingle();
            Container.BindInterfacesAndSelfTo<FSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<Score>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<FinalScoreViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<RebirthViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugView>().FromComponentInNewPrefab(_debugView).UnderTransform(_canvas.transform).AsSingle();
            Container.BindInterfacesAndSelfTo<FinalScoreView>().FromComponentInNewPrefab(_finalScoreView).UnderTransform(_canvas.transform).AsSingle();
            Container.BindInterfacesAndSelfTo<RebirthView>().FromComponentInNewPrefab(_rebirthView).UnderTransform(_canvas.transform).AsSingle();

            Container.Bind<IWeapon>().WithId("first").To<BulletWeapon>().FromResolve().AsCached();
            Container.Bind<IWeapon>().WithId("second").To<LazerWeapon>().FromResolve().AsCached();
            Container.BindInterfacesAndSelfTo<BulletWeapon>().AsCached();
            Container.BindInterfacesAndSelfTo<LazerWeapon>().AsCached();

            Container.BindInstances(_lazerPrefab, _lazerRendererConfig);
            Container.QueueForInject(_lazerPrefab);
            Container.QueueForInject(_lazerRendererConfig);

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

            Container.BindInterfacesAndSelfTo<UnitView>().FromComponentInNewPrefab(_ship).AsSingle().WhenInjectedInto<Game>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_bullet).UnderTransformGroup("Bullets").WhenInjectedInto<BulletWeapon>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_ufo).UnderTransformGroup("UFO").WhenInjectedInto<UFOSpawner>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_smallAsteroid).UnderTransformGroup("SmallAsteroids").WhenInjectedInto<SmallAsteroidSpawner>();
            Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(_bigAsteroid).UnderTransformGroup("BigAsteroids").WhenInjectedInto<BigAsteroidSpawner>();

            Container.BindInterfacesAndSelfTo<UISwitcher>().AsSingle().NonLazy();
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
            _debugView = null;
            _finalScoreView = null;
            _rebirthView = null;
            _bullet = null;
            _smallAsteroid = null;
            _bigAsteroid = null;
            _ufo = null;
            _ship = null;
            _loader.ReleaseAll();
        }
    }
}