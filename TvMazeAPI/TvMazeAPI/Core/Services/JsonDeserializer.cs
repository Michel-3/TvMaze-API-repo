using System.Text.Json;

namespace TvMazeAPI.Core.Services
{
    public class JsonDeserializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
