using System;
using System.Diagnostics;
using UnityEngine;

public class Unit
{
    public event Action<Unit> DeadEvent;

    public TransformData Data { get; set; }

    private readonly IMovement _movement;
    private IBorderHandler _borderHandler;
    private IDieEffect _dieEffect;

    public Unit(IMovement movement, IBorderHandler borderHandler, IDieEffect dieEffect, TransformData data = default)
    {
        Data = data;
        _movement = movement;
        _borderHandler = borderHandler;
        _dieEffect = dieEffect;
    }

    public void Update(float deltaTime)
    {
        Data = _movement.Update(Data, deltaTime);
        Data = _borderHandler.Update(Data);
    }

    public void Die(bool withEffect = true)
    {
        if(withEffect)
            _dieEffect.Die(this);
        DeadEvent?.Invoke(this);
    }
}