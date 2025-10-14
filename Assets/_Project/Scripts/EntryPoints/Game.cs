using Asteroids.Core;
using Asteroids.Core.Weapon;
using Asteroids.FSMachine;
using UnityEngine;
using Zenject;

public class Game : ITickable
{
    private AsteroidsSystem _asteroidSystem;
    private UFOSystem _ufoSystem;
    private Arsenal _arsenal;
    private CompositeUnit _ship;
    private FSM _fsm;

    public Game(AsteroidsSystem asteroidSystem, UFOSystem ufoSystem, Arsenal arsenal, CompositeUnit ship, FSM fsm)
    {
        _asteroidSystem = asteroidSystem;
        _ufoSystem = ufoSystem;
        _arsenal = arsenal;
        _ship = ship;
        _fsm = fsm;

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
        _asteroidSystem.Clear();
        _ufoSystem.Clear();
        _arsenal.Clear();
        _ship.Unit.Data = new TransformData();
        _ship.Unit.OnDied += OnShipDied;
        _fsm.SwitchState(StateEnum.Play);
    }

    private void OnShipDied(Unit unit, bool real)
    {
        _ship.Unit.OnDied -= OnShipDied;
        _fsm.SwitchState(StateEnum.Score);
    }
}