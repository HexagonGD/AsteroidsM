using UnityEngine;

public class UnitViewFactory : IFactory<UnitView>
{
    private readonly UnitView _unitViewPrefab;

    public UnitViewFactory(UnitView unitViewPrefab)
    {
        _unitViewPrefab = unitViewPrefab;
    }

    public UnitView Get()
    {
        return GameObject.Instantiate(_unitViewPrefab);
    }
}