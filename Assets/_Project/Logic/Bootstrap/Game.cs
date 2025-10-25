using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Units.Core;
using Asteroids.Logic.Common.Units.Implementation;
using Asteroids.Logic.Common.Weapon.Implementation;
using Asteroids.Logic.FSMachine;
using Asteroids.Logic.Remote;
using R3;
using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Unit = Asteroids.Logic.Common.Units.Core.Unit;

namespace Asteroids.Logic.Bootstrap
{
    public class Game : ITickable, IInitializable, IDisposable
    {
        private readonly CompositeUnitRepository _unitRepository;
        private readonly SpawnersController _enemyController;
        private readonly Arsenal _arsenal;
        private readonly CompositeUnit _ship;
        private readonly FSM _fsm;
        private readonly Score _score;
        private readonly RemoteConfigsLoader _remoteConfigsLoader;

        private IDisposable _disposable;

        public Game(CompositeUnitRepository unitRepository, SpawnersController enemyController,
                    Arsenal arsenal, Ship ship, UnitView shipView, FSM fsm, Score score, RemoteConfigsLoader remoteConfigsLoader)
        {
            _unitRepository = unitRepository;
            _enemyController = enemyController;
            _arsenal = arsenal;
            _ship = new CompositeUnit(ship, shipView);
            _fsm = fsm;
            _score = score;
            _remoteConfigsLoader = remoteConfigsLoader;
        }

        public void Initialize()
        {
            _disposable = _remoteConfigsLoader.ConfigsLoaded.Where(x => x).Subscribe(x => ConfigsLoaderCompleteLoaded(x));
            _unitRepository.OnUnitRegistered += UnitRegisteredHandler;
            _unitRepository.Ship = _ship;
            _fsm.OnStateChanged += StateChangedHandler;
        }

        public void Tick()
        {
            if (_fsm.State == StateEnum.Play)
            {
                _enemyController.Update(Time.deltaTime);
                foreach (var unit in _unitRepository.Units.ToArray())
                    unit.Update(Time.deltaTime);
                _arsenal.Update(Time.deltaTime);
            }
        }

        private void ConfigsLoaderCompleteLoaded(bool result)
        {
            _disposable.Dispose();
            _fsm.SwitchState(StateEnum.Play);
        }

        private void UnitRegisteredHandler(CompositeUnit unit)
        {
            unit.Unit.OnDied += EnemyDiedHandler;
        }

        private void StateChangedHandler(StateEnum state)
        {
            if (state == StateEnum.Play)
                RunGame();
        }

        private void RunGame()
        {
            Clear();

            _ship.Unit.OnDied += ShipDiedHandler;
        }

        private void EnemyDiedHandler(Unit unit, bool real)
        {
            if (real)
                _score.Current.Value++;
            unit.OnDied -= EnemyDiedHandler;
        }

        private void Clear()
        {
            _score.Current.Value = 0;
            _enemyController.Clear();
            _arsenal.Clear();
            _ship.Unit.Data = new TransformData();
        }

        private void ShipDiedHandler(Unit unit, bool real)
        {
            _ship.Unit.OnDied -= ShipDiedHandler;
            _fsm.SwitchState(StateEnum.Score);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}