using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesLoader
{
    private Dictionary<AssetReference, BaseHandleContainer> _handlers = new();

    public async UniTask<T> LoadInternal<T>(AssetReference reference)
    {
        var handle = Addressables.LoadAssetAsync<T>(reference);
        _handlers[reference] = new HandleContainer<T>(handle);
        await handle.Task.AsUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
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

    public class HandleContainer<T> : BaseHandleContainer
    {
        private readonly AsyncOperationHandle<T> _handle;

        public HandleContainer(AsyncOperationHandle<T> handle)
        {
            _handle = handle;
        }

        public override void Release()
        {
            _handle.Release();
        }
    }

    public abstract class BaseHandleContainer
    {
        public abstract void Release();
    }
}