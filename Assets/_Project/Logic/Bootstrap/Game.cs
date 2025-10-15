using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Units;
using Asteroids.Logic.Common.Weapon.Implementation;
using Asteroids.Logic.FSMachine;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Bootstrap
{
    public class Game : ITickable, IInitializable
    {
        private readonly EnemyController _enemyController;
        private readonly Arsenal _arsenal;
        private readonly CompositeUnit _ship;
        private readonly FSM _fsm;
        private readonly Score _score;

        public Game(EnemyController enemyController, Arsenal arsenal, CompositeUnit ship, FSM fsm, Score score)
        {
            _enemyController = enemyController;
            _arsenal = arsenal;
            _ship = ship;
            _fsm = fsm;
            _score = score;
        }

        public void Initialize()
        {
            _enemyController.OnEnemyDied += EnemyDiedHandler;
            _fsm.OnStateChanged += StateChangedHandler;
            _fsm.SwitchState(StateEnum.Play);
        }

        private void StateChangedHandler(StateEnum state)
        {
            if (state == StateEnum.Play)
                RunGame();
        }

        public void Tick()
        {
            if (_fsm.State == StateEnum.Play)
            {
                _enemyController.Update(Time.deltaTime);
                _ship.Update(Time.deltaTime);
                _arsenal.Update(Time.deltaTime);
            }
        }

        private void RunGame()
        {
            Clear();

            _ship.Unit.OnDied += OnShipDied;
        }

        private void EnemyDiedHandler()
        {
            _score.Value.Value++;
        }

        private void Clear()
        {
            _score.Value.Value = 0;
            _enemyController.Clear();
            _arsenal.Clear();
            _ship.Unit.Data = new TransformData();
        }

        private void OnShipDied(Unit unit, bool real)
        {
            _ship.Unit.OnDied -= OnShipDied;
            _fsm.SwitchState(StateEnum.Score);
        }
    }
}