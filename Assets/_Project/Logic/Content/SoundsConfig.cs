using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Asteroids.Logic.Content
{
    [CreateAssetMenu(fileName = "SoundsConfig", menuName = "Configs/SoundsConfig")]
    public class SoundsConfig : ScriptableObject
    {
        [field: SerializeField] public AudioClip BGClip { get; private set; }
        [field: SerializeField] public AudioClip ShipDestroyedClip { get; private set; }
        [field: SerializeField] public AudioClip AsteroidDestroyedClip { get; private set; }
        [field: SerializeField] public AudioClip UFODestroyedClip { get; private set; }
        [field: SerializeField] public AudioClip ShotClip { get; private set; }
        [field: SerializeField] public AudioClip LazerClip { get; private set; }
    }
}