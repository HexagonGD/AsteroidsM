using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class UniTaskExtension
{
    public static async UniTask<T> DoUntilComplete<T>(Func<UniTask<T>> task, float delayBetweenTry, int maxTry = -1, CancellationToken token = default)
    {
        int tries = 0;
        bool success = false;
        T result = default;

        while(success == false && token.IsCancellationRequested == false &&
             (maxTry == -1 || tries++ < maxTry))
        {
            success = true;

            try
            {
                result = await task();
            }
            catch(Exception e)
            {
                success = false;
                Debug.LogError(e);
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenTry));
            }
        }

        return result;
    }

    public static async UniTask DoUntilComplete(Func<Task> task, float delayBetweenTry, CancellationToken token = default)
    {
        var result = false;
        while (result == false && token.IsCancellationRequested == false)
        {
            result = true;

            try
            {
                await task().AsUniTask();
            }
            catch (Exception e)
            {
                result = false;
                Debug.LogError(e);
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenTry));
            }
        }
    }

    public static async UniTask DoUntilComplete(Func<UniTask> task, float delayBetweenTry, CancellationToken token = default)
    {
        var result = false;
        while (result == false && token.IsCancellationRequested == false)
        {
            result = true;

            try
            {
                await task();
            }
            catch (Exception e)
            {
                result = false;
                Debug.LogError(e);
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenTry));
            }
        }
    }

    public static async UniTask<string> DoUntilComplete(Func<UniTask<string>> task, float delayBetweenTry, CancellationToken token = default)
    {
        var result = false;
        var strResult = string.Empty;

        while (result == false && token.IsCancellationRequested == false)
        {
            result = true;

            try
            {
                strResult = await task();
            }
            catch (Exception e)
            {
                result = false;
                Debug.LogError(e);
                await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenTry));
            }
        }

        return strResult;
    }
}