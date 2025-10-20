using Asteroids.Logic.Common.Services.Saving.Core;
using UnityEngine;

namespace Asteroids.Logic.Common.Services.Saving.Implementation
{
    public class PlayerPrefsSaveManager : ISaveManager
    {
        private const string KEY = "SaveDataKey";

        public void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(KEY, json);
            PlayerPrefs.Save();
        }

        public bool TryLoad(out SaveData data)
        {
            data = new();
            if (PlayerPrefs.HasKey(KEY) == false)
                return false;

            var json = PlayerPrefs.GetString(KEY);
            JsonUtility.FromJsonOverwrite(json, data);
            return true;
        }
    }
}