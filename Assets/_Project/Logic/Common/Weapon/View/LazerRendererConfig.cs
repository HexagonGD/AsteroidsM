using UnityEngine;

namespace Asteroids.Logic.Common.Weapon.View
{
    [CreateAssetMenu(fileName = "LazerRendererConfig", menuName = "Configs/LazerRendererConfig")]
    public class LazerRendererConfig : ScriptableObject
    {
        [field: SerializeField] public float LifeTime { get; private set; }
        [field: SerializeField] public float Length { get; private set; }
        [field: SerializeField] public AnimationCurve WidthCurve { get; private set; }
    }
}