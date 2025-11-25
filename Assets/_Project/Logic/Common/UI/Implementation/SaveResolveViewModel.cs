using Asteroids.Logic.Common.Services.Saving;
using Asteroids.Logic.Common.Services.Saving.Core;
using Cysharp.Threading.Tasks;
using R3;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class SaveResolveViewModel
    {
        private readonly ISaveSystem _saveSystem;
        private ReactiveProperty<bool> _resolving = new(false);

        public ReadOnlyReactiveProperty<bool> NeedResolve => _saveSystem.NeedResolve;
        public ReadOnlyReactiveProperty<bool> Resolving => _resolving;
        public SaveData LocalData => _saveSystem.Data.CurrentValue;
        public SaveData CloudData => _saveSystem.LastSessionCloudData;

        public SaveResolveViewModel(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public async UniTask Resolve(SaveData data)
        {
            if (_resolving.Value)
                return;

            _resolving.Value = true;
            await _saveSystem.Resolve(data);
            _resolving.Value = false;
        }
    }
}