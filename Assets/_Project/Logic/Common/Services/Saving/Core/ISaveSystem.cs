using Cysharp.Threading.Tasks;
using R3;

namespace Asteroids.Logic.Common.Services.Saving.Core
{
    public interface ISaveSystem
    {
        public ReadOnlyReactiveProperty<SaveData> Data { get; }
        public ReadOnlyReactiveProperty<bool> NeedResolve { get; }
        public SaveData LastSessionLocalData { get; }
        public SaveData LastSessionCloudData { get; }

        public UniTask SaveAsync(SaveData data);
        public UniTask<SaveData> LoadAsync(SaveData data);
        public UniTask Resolve(SaveData data);
    }
}