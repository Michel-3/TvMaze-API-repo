using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TvMazeAPI.Core.Models;
using TvMazeAPI.Core.Services;

namespace TvMaze.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TvMazeController : ControllerBase
    {
        private readonly TvShowValidator _validator;
        private readonly RateLimiter _rateLimiter;
        private readonly JsonDeserializer _jsonDeserializer;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TvMazeController> _logger;

        public TvMazeController(
            HttpClient httpClient,
            RateLimiter rateLimiter,
            JsonDeserializer jsonDeserializer,
            TvShowValidator validator,
            ILogger<TvMazeController> logger)
        {
            _httpClient = httpClient;
            _rateLimiter = rateLimiter;
            _jsonDeserializer = jsonDeserializer;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet("shows")]
        public async Task<IActionResult> GetShowsByDate([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var (isValid, errorMessage) = _validator.ValidateMonthAndYear(month, year);

                if (!isValid)
                {
                    return StatusCode(500, errorMessage);
                }

                var showInfoList = new List<ShowInfo>();

                _httpClient.BaseAddress = new Uri("http://api.tvmaze.com/");

                // day <= DateTime.DaysInMonth(year, month) hele maand
                for (int day = 1; day <= 2; day++)
                {
                    var apiUrl = $"schedule?date={year}-{month:D2}-{day:D2}";

                    var response = await _rateLimiter.ApiRateLimiter(apiUrl);

                    var doc = _jsonDeserializer.Deserialize<JsonDocument>(response);

                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var showElement in doc.RootElement.EnumerateArray())
                        {
                            var airdateProperty = showElement.GetProperty("airdate");
                            var showProperty = showElement.GetProperty("show");
                            var idProperty = showElement.GetProperty("id");
                            var nameProperty = showElement.GetProperty("name");

                            if (idProperty.ValueKind == JsonValueKind.Number)
                            {
                                var showAirDate = airdateProperty.GetString();
                                var showId = showProperty.GetProperty("id").GetInt32();
                                var showName = nameProperty.GetString();
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

                                    showInfoList.Add(new ShowInfo { ShowAirDate = showAirDate, ShowId = showId, ShowName = showName, Cast = castList });
                                    _logger.LogInformation($"Processing show: {showId}, {showName}");
                                }
                                catch (HttpRequestException ex)
                                {
                                    _logger.LogError($"Error retrieving cast for show {showId}: {ex.Message}");
                                }
                            }
                        }
                    }
                }

                return Ok(showInfoList);
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, $"An error occurred while retrieving the data");
            }
        }
    }
}