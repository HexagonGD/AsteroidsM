using Asteroids.Logic.Common.Services.Saving.Core;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Asteroids.Logic.Common.Services.Saving.Implementation
{
    public class PlayerPrefsDataStorage : IDataStorage
    {
        public UniTask InitializeAsync(CancellationToken token = default)
        {
            return UniTask.CompletedTask;
        }

        public UniTask<string> ReadAsync(string key, CancellationToken token = default)
        {
            return UniTask.FromResult(PlayerPrefs.GetString(key, string.Empty));
        }

        public UniTask WriteAsync(string key, string serializedData, CancellationToken token = default)
        {
            PlayerPrefs.SetString(key, serializedData);
            return UniTask.CompletedTask;
        }

        public UniTask DeleteAsync(string key, CancellationToken token = default)
        {
            PlayerPrefs.DeleteKey(key);
            return UniTask.CompletedTask;
        }

        public UniTask<bool> ExistsAsync(string key, CancellationToken token = default)
        {
            return UniTask.FromResult(PlayerPrefs.HasKey(key));
        }
    }
}