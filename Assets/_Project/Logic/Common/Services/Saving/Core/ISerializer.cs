namespace Asteroids.Logic.Common.Services.Saving.Core
{
    public interface ISerializer
    {
        public string Serialize<T>(T data);
        public T Deserialize<T>(string sData);
    }
}