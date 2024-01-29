using Newtonsoft.Json;

namespace TvMazeAPI.Core.Services
{
    public class JsonDeserializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
