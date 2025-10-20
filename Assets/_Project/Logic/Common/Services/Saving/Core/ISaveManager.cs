namespace Asteroids.Logic.Common.Services.Saving.Core
{
    public interface ISaveManager
    {
        public void Save(SaveData data);
        public bool TryLoad(out SaveData data);
    }
}