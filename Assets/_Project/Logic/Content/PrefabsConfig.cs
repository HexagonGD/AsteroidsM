using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Asteroids.Logic.Content
{
    [CreateAssetMenu(fileName = "PrefabsConfig", menuName = "Configs/PrefabsConfig")]
    public class PrefabsConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference ShipPrefab {  get; private set; }
        [field: SerializeField] public AssetReference UFOPrefab { get; private set; }
        [field: SerializeField] public AssetReference SmallAsteroidPrefab { get;    set; }
        [field: SerializeField] public AssetReference BigAsteroidPrefab { get; private set; }
        [field: SerializeField] public AssetReference BulletPrefab { get; private set; }
        [field: SerializeField] public AssetReference DebugViewPrefab { get; private set; }
        [field: SerializeField] public AssetReference FinalScoreViewPrefab { get; private set; }
        [field: SerializeField] public AssetReference RebirthViewPrefab { get; private set; }
        [field: SerializeField] public AssetReference MainMenuViewPrefab { get; private set; }
        [field: SerializeField] public AssetReference SaveResolveView { get; private set; }
    }
}