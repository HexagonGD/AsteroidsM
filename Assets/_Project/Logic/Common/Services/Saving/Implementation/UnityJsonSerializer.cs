using Asteroids.Logic.Common.Services.Saving.Core;
using UnityEngine;

namespace Asteroids.Logic.Common.Services.Saving.Implementation
{
    public class UnityJsonSerializer : ISerializer
    {
        public T Deserialize<T>(string sData)
        {
            return JsonUtility.FromJson<T>(sData);
        }

        public string Serialize<T>(T data)
        {
            return JsonUtility.ToJson(data);
        }
    }
}