using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using TvMazeAPI.Core.Models;


[ApiController]
[Route("[controller]")]
public class DateController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DateController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("GetShowsByDate")]
    public async Task<IActionResult> GetShowsByDate(int month, int year)
    {
        if (!IsValidMonth(month))
        {
            return StatusCode(500, "Onjuiste maand waarde. De maand moet tussen de 1 en 12 zijn");
        }

        if (!IsValidYear(year))
        {
            return StatusCode(500, "Onjuist jaar waarde. Het jaar moet tussen 1951 en 2024");
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = $"https://api.tvmaze.com/schedule?country=US&date={year}-{month:D2}-01";

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
               // var tvMazeDataList = await response.Content.ReadAsAsync<List<TvMazeData>>(new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

                //var showNames = tvMazeDataList.Select(show => show.Name).ToList();

                return Ok(response);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error from TVMaze API");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }

    }

    private bool IsValidMonth(int month)
    {
        return month >= 1 & month <= 12;
    }

    private bool IsValidYear(int year)
    {
        return year >= 1951;
    }
    
}