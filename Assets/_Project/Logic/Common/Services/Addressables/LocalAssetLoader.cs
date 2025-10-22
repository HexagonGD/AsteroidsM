using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Asteroids.Logic.Common.Services
{
    public class LocalAssetLoader
    {
        private GameObject _cachedObject;

        public async UniTask<T> LoadInternal<T>(string assetID)
        {
            var handle = Addressables.InstantiateAsync(assetID);
            _cachedObject = await handle.ToUniTask();

            if (_cachedObject.TryGetComponent<T>(out var component) == false)
                throw new System.NullReferenceException();

            return component;
        }

        public void ReleaseInternal()
        {
            if (_cachedObject == null)
                return;

            _cachedObject.SetActive(false);
            Addressables.ReleaseInstance(_cachedObject);
            _cachedObject = null;
        }
    }
}