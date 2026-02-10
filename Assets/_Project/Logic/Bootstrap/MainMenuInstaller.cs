using Asteroids.Logic.Common.Services;
using Asteroids.Logic.Common.UI.Implementation;
using Asteroids.Logic.Content;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Bootstrap
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private Transform _uiContainer;
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private SceneContext _sceneContext;

        private LocalPrefabLoader _loader = new();
        private MainMenuView _menuView;
        private SaveResolveView _resolveView;

        private async void Awake()
        {
            _menuView = await _loader.LoadInternal<MainMenuView>(_prefabsConfig.MainMenuViewPrefab);
            _resolveView = await _loader.LoadInternal<SaveResolveView>(_prefabsConfig.SaveResolveView);
            _sceneContext.Run();
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuView>().FromComponentInNewPrefab(_menuView).UnderTransform(_uiContainer).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SaveResolveView>().FromComponentInNewPrefab(_resolveView).UnderTransform(_uiContainer).AsSingle().NonLazy();
        }

        private void OnDestroy()
        {
            _menuView = null;
            _resolveView = null;
            _loader.ReleaseAll();
        }
    }
}