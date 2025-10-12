using System;
using System.Collections.Generic;
using UnityEngine;

public class UFOSystem
{
    public event Action UFODeadEvent;

    private CompositeFactory _ufoFactory;

    private Config _config;
    private LoopTimer _timer;
    private PlayZone _playZone;
    private SpawnOutsideGameZone _spawnPosition;

    private List<CompositeUnit> _ufo = new();

    public UFOSystem(Config config, Unit target, PlayZone playZone, SpawnOutsideGameZone spawnPosition)
    {
        _config = config;
        _playZone = playZone;
        _spawnPosition = spawnPosition;

        _timer = new LoopTimer(config.TimeForSpawn, config.AccumulatedTime);
        _timer.LoopEvent += SpawnUFO;

        var linearMovement = new MoveToTarget(target, _config._movementConfig);
        var ignoreBorder = new IgnoreBorder();
        var withoutEffect = new WithoutDieEffect();

        var ufoUnitFactory = new UnitFactory(linearMovement, ignoreBorder, withoutEffect);
        var ufoUnitViewFactory = new UnitViewFactory(_config.UFOPrefab);

        _ufoFactory = new CompositeFactory(ufoUnitFactory, ufoUnitViewFactory);
    }

    public void Update(float deltaTime)
    {
        _timer.Update(deltaTime);
        for (var i = _ufo.Count - 1; i >= 0; i--)
        {
            _ufo[i].Update(deltaTime);
        }
    }

    public void Clear()
    {
        for (var i = _ufo.Count - 1; i >= 0; i--)
            _ufo[i].Unit.Die(false);

        _timer.LoopEvent -= SpawnUFO;
        _timer = new LoopTimer(_config.TimeForSpawn, _config.AccumulatedTime);
        _timer.LoopEvent += SpawnUFO;
    }

    private void SpawnUFO()
    {
        var ufo = _ufoFactory.Get();
        var data = new TransformData();
        data.Position = _spawnPosition.GetSpawnPosition(_playZone);
        ufo.Unit.Data = data;
        ufo.Unit.DeadEvent += OnUFODead;
        _ufo.Add(ufo);
    }

    private void OnUFODead(Unit unit)
    {
        var index = _ufo.FindIndex(x => x.Unit == unit);
        _ufo[index].Unit.DeadEvent -= OnUFODead;
        _ufoFactory.Release(_ufo[index]);
        _ufo.RemoveAt(index);
        UFODeadEvent?.Invoke();
    }

    [System.Serializable]
    public class Config
    {
        [Min(0)] public float TimeForSpawn;
        public float AccumulatedTime;

        public UnitView UFOPrefab;
        public MoveToTarget.Config _movementConfig;
    }
}