using System.Text.Json;
using TvMazeAPI.Core.Services.Interfaces;

namespace TvMazeAPI.Core.Services.Implementations
{
    public class ShowDataService
    {
        private readonly IRateLimiter _rateLimiter;
        private readonly IJsonDeserializer _jsonDeserializer;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShowDataService> _logger;
        private readonly IDataCheckService _dataCheckService;

        public ShowDataService(
            IRateLimiter rateLimiter,
            IJsonDeserializer jsonDeserializer,
            HttpClient httpClient,
            ILogger<ShowDataService> logger,
            IDataCheckService dataCheckService)
        {
            _rateLimiter = rateLimiter;
            _jsonDeserializer = jsonDeserializer;
            _httpClient = httpClient;
            _logger = logger;
            _dataCheckService = dataCheckService;
        }

        public async Task<List<ShowInfo>> RetrieveShowsAsync(int year, int month)
        {
            var showInfoList = new List<ShowInfo>();

            _httpClient.BaseAddress = new Uri("http://api.tvmaze.com");

            // day <= DateTime.DaysInMonth(year, month) hele maand
            for (int day = 1; day<= 2; day++)
            {
                var apiUrl = $"schedule?date={year}-{month:D2}-{day:D2}";

                var response = await _rateLimiter.ApiRateLimiter(apiUrl);

                var doc = _jsonDeserializer.Deserialize<JsonDocument>(response);

                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var showElement in doc.RootElement.EnumerateArray())
                    {
                        var idProperty = showElement.GetProperty("id");

                        if (idProperty.ValueKind == JsonValueKind.Number)
                        {
                            var showId = showElement.GetProperty("show").GetProperty("id").GetInt32();
                            var showExists = await _dataCheckService.ShowExistsAsync(showId);

                            if (!showExists)
                            {
                                var showAirDate = showElement.GetProperty("airdate").GetString();
                                var showName = showElement.GetProperty("show").GetProperty("name").GetString();

                                try
                                {
                                    var castResponse = await _rateLimiter.ApiRateLimiter($"shows/{showId}/cast");

                                    _logger.LogInformation($"API Response for show {showId}/cast: {castResponse}");

                                    var castData = _jsonDeserializer.Deserialize<JsonDocument>(castResponse);
                                    var castList = castData.RootElement.EnumerateArray()
                                        .Select(actor => new CastMember
                                        {
                                            PersonId = actor.GetProperty("person").TryGetProperty("id", out var idElement) ? idElement.GetInt32() : 0,
                                            PersonName = actor.GetProperty("person").TryGetProperty("name", out var nameElement) ? nameElement.GetString() : "Unknown"
                                        })
                                        .ToList();

                                    showInfoList.Add(new ShowInfo { showAirDate = showAirDate, showId = showId, ShowName = showName, castData = castList });
                                    _logger.LogInformation($"Processing show: {showId}, {showName}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
