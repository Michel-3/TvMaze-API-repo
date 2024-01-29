using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TvMazeAPI.Core.Models;

namespace TvMaze.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TvMazeController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TvMazeController> _logger;

        public TvMazeController(HttpClient httpClient, ILogger<TvMazeController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet("shows")]
        public async Task<IActionResult> GetShowsByDate([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                if (!IsValidMonth(month))
                {
                    return StatusCode(500, "Onjuiste maand waarde. De maand moet tussen de 1 en 12 zijn.");
                }

                if (!IsValidYear(year))
                {
                    return StatusCode(500, "Onjuist jaar waarde. Het jaar moet tussen 1951 en 2100 zijn.");
                }

                var showInfoList = new List<ShowInfo>();

                _httpClient.BaseAddress = new Uri("http://api.tvmaze.com/");

                // day <= DateTime.DaysInMonth(year, month) hele maand
                for (int day = 1; day <= 2; day++)
                {
                    var apiUrl = $"schedule?date={year}-{month:D2}-{day:D2}";

                    var response = await ApiRateLimiter(apiUrl);


                    using (JsonDocument doc = JsonDocument.Parse(response))
                    {
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
                                        var castResponse = await ApiRateLimiter($"shows/{showId}/cast");

                                        _logger.LogInformation($"API Response for show {showId}/cast: {castResponse}");


                                        var castData = JsonDocument.Parse(castResponse);
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
                }

                return Ok(showInfoList);
            }

            catch (HttpRequestException)
            {
                return StatusCode(500, $"An error occurred while retrieving the data");
            }
        }
        private bool IsValidMonth(int month)
        {
            return month >= 1 && month <= 12;
        }

        private bool IsValidYear(int year)
        {
            return year >= 1951 && year <= 2100;
        }

        private async Task<string> ApiRateLimiter(string apiUrl)
        {
           await Task.Delay(500);
 
           var response = await _httpClient.GetStringAsync(apiUrl);

           return response;
        }
    }
}


   
