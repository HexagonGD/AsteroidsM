using UnityEngine;

namespace Asteroids.Logic.Common.Movement.Implementation
{
    [CreateAssetMenu(fileName = "AccelerationMovementConfig", menuName = "Configs/AccelerationMovementConfig")]
    public class AccelerationMovementConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float IncreaseVelocity { get; private set; }
        [field: SerializeField, Min(0)] public float DecreaseVelocity { get; private set; }
        [field: SerializeField, Min(0)] public float MaxVelocity { get; private set; }
        [field: SerializeField, Min(0)] public float RotateSpeed { get; private set; }
    }
}