using Cysharp.Threading.Tasks;
using System.Threading;

namespace Asteroids.Logic.Common.Services.Saving.Core
{
    public interface IDataStorage
    {
        UniTask InitializeAsync(CancellationToken token = default);
        UniTask<string> ReadAsync(string key, CancellationToken token = default);
        UniTask WriteAsync(string key, string serializedData, CancellationToken token = default);
        UniTask DeleteAsync(string key, CancellationToken token = default);
        UniTask<bool> ExistsAsync(string key, CancellationToken token = default);
    }
}