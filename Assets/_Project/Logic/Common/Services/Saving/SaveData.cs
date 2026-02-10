using System;

namespace Asteroids.Logic.Common.Services.Saving
{
    public struct SaveData
    {
        public long Timestamp;
        public int BestScore;
        public bool AdsDisabled;

        public static SaveData Default => new SaveData() { Timestamp = 0, BestScore = 0, AdsDisabled = false };
    }
}