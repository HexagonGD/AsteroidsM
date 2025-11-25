using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asteroids.Logic.Common.Services
{
    public class LocalPrefabLoader
    {
        private Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _handlers = new();

        public async UniTask<T> LoadInternal<T>(AssetReference reference)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(reference);
            _handlers[reference] = handle;
            await handle.Task.AsUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (handle.Result.TryGetComponent<T>(out var component) == false)
                    throw new System.Exception($"Failed to get component {typeof(T)}");
                return component;
            }
            else
            {
                throw new System.Exception($"Failed to load asset {reference.ToString()}");
            }
        }

        public void ReleaseAll()
        {
            foreach (var handler in _handlers)
                handler.Value.Release();
            _handlers.Clear();
        }

        public void ReleaseInternal(AssetReference reference)
        {
            if (_handlers.ContainsKey(reference))
            {
                _handlers[reference].Release();
                _handlers.Remove(reference);
            }
        }
    }
}