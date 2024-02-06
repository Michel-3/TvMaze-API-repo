namespace TvMazeAPI.Core.Services.Interfaces
{
    public interface IJsonDeserializer
    {
        T Deserialize<T>(string json);
    }
}