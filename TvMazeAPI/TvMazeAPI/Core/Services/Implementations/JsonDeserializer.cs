using System.Text.Json;
using TvMazeAPI.Core.Services.Interfaces;

namespace TvMazeAPI.Core.Services.Implementations
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
