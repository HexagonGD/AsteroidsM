using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroids.Logic.Common.Services
{
    public class LocalPrefabLoader
    {
        private AsyncOperationHandle<GameObject> _handle;

        public T LoadInternal<T>(AssetReference reference)
        {
            _handle = Addressables.LoadAssetAsync<GameObject>(reference);
            _handle.WaitForCompletion();

            if (_handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (_handle.Result.TryGetComponent<T>(out var component) == false)
                    throw new System.Exception($"Failed to get component {typeof(T)}");
                return component;
            }
            else
            {
                throw new System.Exception($"Failed to load asset {reference.ToString()}");
            }
        }

        public void ReleaseInternal()
        {
            Addressables.ReleaseInstance(_handle);
        }
    }
}