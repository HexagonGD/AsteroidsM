public class UnitFactory : IFactory<Unit>
{
    private readonly IMovement _movement;
    private readonly IBorderHandler _borderHandler;
    private readonly IDieEffect _dieEffect;

    public UnitFactory(IMovement movement, IBorderHandler borderHandler, IDieEffect dieEffect)
    {
        _movement = movement;
        _borderHandler = borderHandler;
        _dieEffect = dieEffect;
    }

    public Unit Get()
    {
        return new Unit(_movement, _borderHandler, _dieEffect);
    }
}