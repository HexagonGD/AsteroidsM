using Asteroids.Core;
using Asteroids.Core.BorderHandler.Implementation;
using Asteroids.Core.BorderHandler.Interface;
using Asteroids.Core.Movement.Implementation;
using Asteroids.Core.Movement.Interface;
using Asteroids.Core.Weapon;
using Asteroids.Core.Weapon.Implementation;
using Asteroids.Core.Weapon.Interface;
using Asteroids.Factory.Implementation;
using Asteroids.UI;
using Asteroids.View;
using Asteroids.FSMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids
{
    public class Bootstrap : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] AsteroidsSystem.Config _asteroidSystemConfig;
        [SerializeField] UFOSystem.Config _ufoSystemConfig;
        [SerializeField] private AccelerationMovement.SpeedData _shipMovementConfig;
        [SerializeField] private MoveToTarget.Config _ufoMovementConfig;

        [Header("Weapon Configs")]
        [SerializeField] private BulletWeapon.Config _bulletWeaponConfig;
        [SerializeField] private LazerWeapon.Config _lazerWeaponConfig;
        [SerializeField] private LazerRenderer.Config _lazerRendererConfig;

        [Header("UI")]
        [SerializeField] private ScoreUI _gameScoreUI;
        [SerializeField] private ScoreUI _finalScoreUI;
        [SerializeField] private ShipInfoUI _shipInfoUI;
        [SerializeField] private Button _startGameButton;

        [Header("Prefabs")]
        [SerializeField] private UnitView _bulletPrefab;
        [SerializeField] private UnitView _shipPrefab;
        [SerializeField] private UnitView _smallAsteroidPrefab;
        [SerializeField] private UnitView _bigAsteroidPrefab;
        [SerializeField] private UnitView _ufoPrefab;
        [SerializeField] private LineRenderer _lazerPrefab;

        public override void InstallBindings()
        {
            var playZone = new PlayZone(_camera);

            var shipFactory = BuildCompositeFactory(new AccelerationMovement(_shipMovementConfig), new ScreenBorderTeleport(playZone), _shipPrefab);
            var ship = shipFactory.Get();
            
            var bulletFactory = BuildCompositeFactory(new LinearMovement(), new IgnoreBorder(), _bulletPrefab);
            var smallAsteroidFactory = BuildCompositeFactory(new LinearMovement(), new IgnoreBorder(), _smallAsteroidPrefab);
            var bigAsteroidFactory = BuildCompositeFactory(new LinearMovement(), new IgnoreBorder(), _bigAsteroidPrefab);
            var ufoFactory = BuildCompositeFactory(new MoveToTarget(ship.Unit, _ufoMovementConfig), new IgnoreBorder(), _ufoPrefab);

            Container.Bind<CompositeFactory>().FromInstance(bulletFactory).WhenInjectedInto<BulletWeapon>();
            Container.Bind<CompositeFactory>().WithId("small").FromInstance(smallAsteroidFactory).WhenInjectedInto<AsteroidsSystem>();
            Container.Bind<CompositeFactory>().WithId("big").FromInstance(bigAsteroidFactory).WhenInjectedInto<AsteroidsSystem>();
            Container.Bind<CompositeFactory>().FromInstance(ufoFactory).WhenInjectedInto<UFOSystem>();

            Container.Bind<CompositeUnit>().FromInstance(ship);
            Container.Bind<Unit>().FromInstance(ship.Unit);
            Container.Bind<PlayZone>().FromInstance(playZone);
            Container.Bind<AsteroidsSystem>().AsSingle();
            Container.Bind<UFOSystem>().AsSingle();
            Container.Bind<Arsenal>().AsSingle();
            Container.Bind<SpawnOutsideGameZone>().AsSingle();
            Container.Bind<LazerRendererController>().AsSingle();
            Container.Bind<FSM>().AsSingle();

            Container.Bind<IWeapon>().WithId("first").To<BulletWeapon>().AsSingle();
            Container.Bind<IWeapon>().WithId("second").To<LazerWeapon>().AsSingle();

            Container.BindInstances(_asteroidSystemConfig, _ufoSystemConfig, _bulletWeaponConfig, _lazerWeaponConfig, _lazerPrefab, _lazerRendererConfig);
            Container.QueueForInject(_asteroidSystemConfig);
            Container.QueueForInject(_ufoSystemConfig);
            Container.QueueForInject(_bulletWeaponConfig);
            Container.QueueForInject(_lazerWeaponConfig);
            Container.QueueForInject(_lazerPrefab);
            Container.QueueForInject(_lazerRendererConfig);
            Container.QueueForInject(ship);
            Container.QueueForInject(ship.Unit);
            Container.QueueForInject(bulletFactory);
            Container.QueueForInject(smallAsteroidFactory);
            Container.QueueForInject(bigAsteroidFactory);
            Container.QueueForInject(ufoFactory);

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