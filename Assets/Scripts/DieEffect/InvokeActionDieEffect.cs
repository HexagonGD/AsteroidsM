using System;

public class InvokeActionDieEffect : IDieEffect
{
    private readonly Action<Unit> _action;

    public InvokeActionDieEffect(Action<Unit> action)
    {
        _action = action;
    }

    public void Die(Unit unit)
    {
        _action?.Invoke(unit);
    }
}