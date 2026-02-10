using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;

public static class UniTaskExtension
{
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