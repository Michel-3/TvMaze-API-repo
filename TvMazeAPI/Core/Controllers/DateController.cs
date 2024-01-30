using Microsoft.AspNetCore.Mvc;
using TvMazeAPI.Core.Model;

namespace TvMazeAPI.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public DateController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("getdate")]
        public IActionResult GetDate([FromQuery] int? month, [FromQuery] int? year)
        {
            if (!IsValidMonth(month) || !IsValidYear(year))
            {
                return StatusCode(500, "Invalide maand of jaar parameter.");
            }

            if (!month.HasValue || !year.HasValue)
            {
               return StatusCode(500, "Maand en jaar parameters zijn verplicht");
            }

            string formattedDate = new DateTime(year.Value, month.Value, 1).ToString("yyyy-MM-dd");

            string apiUrl = $"https://api.tvmaze.com/schedule?country=US&date={formattedDate}";

            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    return Ok(result);
                }

                return StatusCode((int)response.StatusCode, $"Externe API aanvraag gefaald met deze status code: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Fout bij het maken van een externe API-aanvraag: {ex.Message}");
            }
        }

        private bool IsValidMonth(int? month)
        {
            return month >= 1 && month <= 12;
        }

        private bool IsValidYear(int? year)
        {
            return year.HasValue && year.Value >= 2024;
        }
    }
}
