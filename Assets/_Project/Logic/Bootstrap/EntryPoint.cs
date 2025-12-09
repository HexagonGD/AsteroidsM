using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.Services.Sounds;
using Asteroids.Logic.Content;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private AssetReference _soundsConfigReference;

    private SoundsService _soundsService;
    private SceneService _sceneService;
    private AddressablesLoader _loader = new AddressablesLoader();

    [Inject]
    public void Construct(SoundsService soundsService, SceneService sceneService)
    {
        _soundsService = soundsService;
        _sceneService = sceneService;
    }

    public async UniTask Start()
    {
        DontDestroyOnLoad(gameObject);
        await LoadSounds();
        await _sceneService.LoadMainMenuScene();
    }

    private async UniTask LoadSounds()
    {
        var sounds = await UniTaskExtension.DoUntilComplete<SoundsConfig>(() => _loader.LoadInternal<SoundsConfig>(_soundsConfigReference), 1f, 5, destroyCancellationToken);
        _soundsService.ProvideSounds(sounds);
        _soundsService.PlayMusic(MusicType.BG);
    }

    private void OnApplicationQuit()
    {
        _loader.ReleaseAll();
    }
}