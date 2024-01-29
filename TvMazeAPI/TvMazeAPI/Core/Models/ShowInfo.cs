using Newtonsoft.Json;
using TvMazeAPI.Core;

namespace TvMazeAPI.Core.Models
{
    public class ShowInfo
    {
        public ShowInfo()
        {
            Cast = new List<CastMember>();
        }

        public int ShowId { get; set; }
        public string ShowName { get; set; }
        public string ShowAirDate { get; set; }
        public List<CastMember> Cast { get; set; }
    }
}
