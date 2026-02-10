using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using System.Linq;
using Asteroids.Logic.Common.Services.Saving.Core;

namespace Asteroids.Logic.Common.Services.Saving.Implementation
{
    public class UnityCloudDataStorage : IDataStorage
    {
        public async UniTask InitializeAsync(CancellationToken token = default)
        {
            await UniTaskExtension.DoUntilComplete(UnityServices.InitializeAsync, 1f, token);
            await UniTaskExtension.DoUntilComplete(() => AuthenticationService.Instance.SignInAnonymouslyAsync(), 1f, token);
        }

        public async UniTask<string> ReadAsync(string key, CancellationToken token = default)
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
            if (playerData.TryGetValue(key, out var data))
            {
                return data.Value.GetAsString();
            }
            else
            {
                return "";
            }
        }

        public async UniTask WriteAsync(string key, string serializedData, CancellationToken token = default)
        {
            var playerData = new Dictionary<string, object>{
          {key, serializedData}
        };

            try
            {
                await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
                Debug.Log($"Saved data {string.Join(',', playerData)}");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public async UniTask<bool> ExistsAsync(string key, CancellationToken token = default)
        {
            try
            {
                var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
                return keys.Any(x => x.Key == key);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public async UniTask DeleteAsync(string key, CancellationToken token = default)
        {
            try
            {
                await CloudSaveService.Instance.Data.Player.DeleteAsync(key);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}