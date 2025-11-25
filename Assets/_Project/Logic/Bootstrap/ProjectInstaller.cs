using Asteroids.Logic.Ads.Core;
using Asteroids.Logic.Ads.Implementation;
using Asteroids.Logic.Common.Services.Saving.Core;
using Asteroids.Logic.Common.Services.Saving.Implementation;
using Asteroids.Logic.Common.UI.Implementation;
using Asteroids.Logic.Payments.Core;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Bootstrap
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private UnityAdsProvider _adsProviderPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PaymentService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityJsonSerializer>().AsSingle();
            Container.Bind<IDataStorage>().WithId("local").To<PlayerPrefsDataStorage>().AsSingle();
            Container.Bind<IDataStorage>().WithId("cloud").To<UnityCloudDataStorage>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveResolveViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuViewModel>().AsSingle();

            BindAds();
        }

        private void BindAds()
        {
            Container.BindInterfacesAndSelfTo<AdsController>().AsSingle().NonLazy();
            //Container.Bind<IAdsProvider>().To<TestAdsProvider>().AsSingle();
            Container.Bind<IAdsProvider>().To<UnityAdsProvider>().FromComponentInNewPrefab(_adsProviderPrefab).AsSingle();
        }
    }
}