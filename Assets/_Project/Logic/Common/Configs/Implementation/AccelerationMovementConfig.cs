using UnityEngine;

namespace Asteroids.Logic.Common.Configs.Implementation
{
    [CreateAssetMenu(fileName = "AccelerationMovementConfig", menuName = "Configs/AccelerationMovementConfig")]
    public class AccelerationMovementConfig : ScriptableObject, IRemoteConfig
    {
        [field: SerializeField, Min(0)] public float IncreaseVelocity { get; private set; }
        [field: SerializeField, Min(0)] public float DecreaseVelocity { get; private set; }
        [field: SerializeField, Min(0)] public float MaxVelocity { get; private set; }
        [field: SerializeField, Min(0)] public float RotateSpeed { get; private set; }

        public string RemoteName => RemoteNames.ShipMovementConfig;
    }
}