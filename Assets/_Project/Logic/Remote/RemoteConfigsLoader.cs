using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Remote
{
    public class RemoteConfigsLoader : IInitializable
    {
        private readonly IEnumerable<IRemoteConfig> _configs;
        private readonly ReactiveProperty<bool> _configsLoaded = new(false);

        public ReadOnlyReactiveProperty<bool> ConfigsLoaded => _configsLoaded;

        public RemoteConfigsLoader(IEnumerable<IRemoteConfig> configs)
        {
            _configs = configs;
        }

        public void Initialize()
        {
            var dict = new Dictionary<string, object>();
            foreach (var config in _configs)
            {
                Debug.Log(JsonUtility.ToJson(config));
                dict[config.RemoteName] = JsonUtility.ToJson(config);
            }

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(dict).AsUniTask().ContinueWith(() =>
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).AsUniTask().ContinueWith(() =>
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().AsUniTask().ContinueWith(ActivateCompletedHandler))).Forget();
        }

        private void ActivateCompletedHandler(bool result)
        {
            Debug.Log($"Activate result: {result}");

            foreach (var config in _configs)
            {
                var json = FirebaseRemoteConfig.DefaultInstance.GetValue(config.RemoteName).StringValue;
                JsonUtility.FromJsonOverwrite(json, config);
            }

            _configsLoaded.Value = true;
        }
    }
}