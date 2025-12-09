using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService
{
    public async UniTask LoadMainMenuScene()
    {
        await SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public async UniTask LoadGameScene()
    {
        await SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }
}