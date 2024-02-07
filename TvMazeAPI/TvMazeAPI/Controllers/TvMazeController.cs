using Microsoft.AspNetCore.Mvc;
using TvMazeAPI.Core.Services.Interfaces;

namespace TvMaze.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TvMazeController : ControllerBase
    {
        private readonly ITvShowValidator _validator;
        private readonly IActorCalculator _actorCalculator;
        private readonly IDatabaseCheckService _databaseCheckService;
        private readonly IApiDataService _apiDataService;

        public TvMazeController(
            ITvShowValidator validator,
            IActorCalculator actorCalculator,
            IDatabaseCheckService databaseCheckService,
            IApiDataService apiDataService)
        {
            _validator = validator;
            _actorCalculator = actorCalculator;
            _databaseCheckService = databaseCheckService;
            _apiDataService = apiDataService;
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

                bool monthYearExists = await _databaseCheckService.MonthYearDataExistsAysnc(year, month);

                if (!monthYearExists)
                {
                    var showInfoList = await _apiDataService.RetrieveShowsAsync(year, month);

                    await _databaseCheckService.SaveShowInfoListAsync(showInfoList);
                }

                var actorPercentages = await _actorCalculator.CalculateActorPercentagesAsync(month, year);

                return Ok(actorPercentages);

            }
            catch (HttpRequestException)
            {
                return StatusCode(500, $"An error occurred while retrieving the data");
            }
        }
    }
}
