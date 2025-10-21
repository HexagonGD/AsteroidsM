using UnityEngine;

namespace Asteroids.Logic.Common.Weapon.Implementation
{
    [CreateAssetMenu(fileName = "LazerWeaponConfig", menuName = "Configs/LaserWeaponConfig")]
    public class LazerWeaponConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int MaxCharges { get; private set; }
        [field: SerializeField, Min(0)] public int StartCharges { get; private set; }
        [field: SerializeField, Min(0)] public float ReloadChargeTime { get; private set; }
        [field: SerializeField, Min(0)] public float DelayBetweenShots { get; private set; }
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
    }
}