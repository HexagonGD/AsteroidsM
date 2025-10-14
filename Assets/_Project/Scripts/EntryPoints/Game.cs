using Asteroids.Core;
using Asteroids.Core.Weapon;
using Asteroids.FSMachine;
using UnityEngine;
using Zenject;

public class Game : ITickable, IInitializable
{
    private readonly AsteroidsSystem _asteroidSystem;
    private readonly UFOSystem _ufoSystem;
    private readonly Arsenal _arsenal;
    private readonly CompositeUnit _ship;
    private readonly FSM _fsm;
    private readonly Score _score;

    public Game(AsteroidsSystem asteroidSystem, UFOSystem ufoSystem, Arsenal arsenal, CompositeUnit ship, FSM fsm, Score score)
    {
        _asteroidSystem = asteroidSystem;
        _ufoSystem = ufoSystem;
        _arsenal = arsenal;
        _ship = ship;
        _fsm = fsm;
        _score = score;
    }

    public void Initialize()
    {
        _asteroidSystem.OnBigAsteroidDied += OnEnemyDied;
        _asteroidSystem.OnSmallAsteroidDied += OnEnemyDied;
        _ufoSystem.OnUFODied += OnEnemyDied;

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
            _asteroidSystem.Update(Time.deltaTime);
            _ufoSystem.Update(Time.deltaTime);
            _ship.Update(Time.deltaTime);
            _arsenal.Update(Time.deltaTime);
        }
    }

    private void RunGame()
    {
        Clear();
        
        _ship.Unit.OnDied += OnShipDied;
    }

    private void OnEnemyDied()
    {
        _score.Value.Value++;
    }

    private void Clear()
    {
        _score.Value.Value = 0;
        _asteroidSystem.Clear();
        _ufoSystem.Clear();
        _arsenal.Clear();
        _ship.Unit.Data = new TransformData();
    }

    private void OnShipDied(Unit unit, bool real)
    {
        _ship.Unit.OnDied -= OnShipDied;
        _fsm.SwitchState(StateEnum.Score);
    }
}