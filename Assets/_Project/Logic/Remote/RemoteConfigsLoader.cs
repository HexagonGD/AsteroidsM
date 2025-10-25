using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroids.Logic.Remote
{
    public class RemoteConfigsLoader : IInitializable, IDisposable
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
                dict[config.RemoteName] = JsonUtility.ToJson(config);
            }

            FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener += ConfigUpdateListenerEventHandler;
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(dict).AsUniTask().ContinueWith(() =>
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).AsUniTask().ContinueWith(() =>
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().AsUniTask().ContinueWith(ActivateCompletedHandler)));
        }

        private void ConfigUpdateListenerEventHandler(object sender, ConfigUpdateEventArgs e)
        {
            Debug.Log("ConfigUpdateListenerEventHandler");
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().AsUniTask().ContinueWith(UpdateConfigs);

            void UpdateConfigs(bool result)
            {
                Debug.Log($"Activate result: {result}");

                foreach (var remoteName in e.UpdatedKeys)
                {
                    var json = FirebaseRemoteConfig.DefaultInstance.GetValue(remoteName).StringValue;
                    var config = _configs.Where(x => x.RemoteName == remoteName).FirstOrDefault();

                    if (config == null)
                        Debug.LogError($"There's no suitable config for the key {remoteName}");
                    else
                        JsonUtility.FromJsonOverwrite(json, config);
                }
            }
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

        public void Dispose()
        {
            FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener -= ConfigUpdateListenerEventHandler;
        }
    }
}