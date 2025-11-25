using Asteroids.Logic.Common.Services.Saving;
using Asteroids.Logic.Common.Services.Saving.Core;
using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using Zenject;

namespace Asteroids.Logic.Common.Services.Saving.Implementation
{
    public class SaveSystem : IInitializable, IDisposable, ISaveSystem
    {
        private const string KEY = "data";

        private readonly ISerializer _serializer;
        private readonly IDataStorage _localDataStorage;
        private readonly IDataStorage _cloudDataStorage;

        private readonly ReactiveProperty<SaveData> _saveData = new(SaveData.Default);
        private readonly ReactiveProperty<bool> _needResolve = new(false);
        private bool _resolveConfirmed = false;

        private SaveData _lastSessionLocalData;
        private SaveData _lastSessionCloudData;

        private CancellationTokenSource _cts = new();

        public bool Initialized { get; private set; }
        public bool NeedSaveResolve { get; private set; }

        public ReadOnlyReactiveProperty<SaveData> Data => _saveData;
        public SaveData LastSessionLocalData => _lastSessionLocalData;
        public SaveData LastSessionCloudData => _lastSessionCloudData;
        public ReadOnlyReactiveProperty<bool> NeedResolve => _needResolve;

        public SaveSystem([Inject(Id = "local")] IDataStorage localSaveStorage, [Inject(Id = "cloud")] IDataStorage cloudSaveStorage, ISerializer serializer)
        {
            _localDataStorage = localSaveStorage;
            _cloudDataStorage = cloudSaveStorage;
            _serializer = serializer;
        }

        public void Initialize()
        {
            _localDataStorage.InitializeAsync(_cts.Token).ContinueWith(LocalDataStorageInitializedHandler).Forget();
            _cloudDataStorage.InitializeAsync(_cts.Token).ContinueWith(CloudDataStorageInitializedHandler).Forget();
        }

        public async UniTask Resolve(SaveData data)
        {
            _saveData.Value = data;
            _resolveConfirmed = true;
            _needResolve.Value = false;
            await SaveAsync(data);
        }

        public async UniTask SaveAsync(SaveData data)
        {
            data.Timestamp = DateTime.Now.Ticks;
            _saveData.Value = data;
            var sData = _serializer.Serialize(data);
            await _localDataStorage.WriteAsync(KEY, sData, _cts.Token);
            if (_resolveConfirmed)
                await _cloudDataStorage.WriteAsync(KEY, sData, _cts.Token);
        }

        public UniTask<SaveData> LoadAsync(SaveData data)
        {
            return UniTask.FromResult(_saveData.Value);
        }

        private async UniTaskVoid LocalDataStorageInitializedHandler()
        {
            if (_cts.Token.IsCancellationRequested)
                return;

            var json = await _localDataStorage.ReadAsync(KEY, _cts.Token);
            if (string.IsNullOrEmpty(json) == false)
            {
                _lastSessionLocalData = _serializer.Deserialize<SaveData>(json);
                _saveData.Value = _lastSessionLocalData;
            }
        }

        private async UniTaskVoid CloudDataStorageInitializedHandler()
        {
            if (_cts.Token.IsCancellationRequested)
                return;

            var json = await UniTaskExtension.DoUntilComplete(() => _cloudDataStorage.ReadAsync(KEY, _cts.Token), 1f, _cts.Token);
            if (string.IsNullOrEmpty(json) == false)
            {
                _lastSessionCloudData = _serializer.Deserialize<SaveData>(json);
                if (_lastSessionCloudData.Timestamp == _lastSessionLocalData.Timestamp || _lastSessionCloudData.Timestamp == 0)
                {
                    _resolveConfirmed = true;
                    await SaveAsync(_saveData.Value);
                }
                else
                {
                    _needResolve.Value = true;
                }
            }
            else if (_cts != null && _cts.IsCancellationRequested == false)
            {
                _resolveConfirmed = true;
                if (_saveData.CurrentValue.Timestamp != 0)
                    SaveAsync(_saveData.CurrentValue).Forget();
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}