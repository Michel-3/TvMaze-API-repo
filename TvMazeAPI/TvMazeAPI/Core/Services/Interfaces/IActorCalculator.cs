namespace TvMazeAPI.Core.Services.Interfaces
{
    public interface IActorCalculator
    {
        Task<IEnumerable<object>> CalculateActorPercentagesAsync(int year, int month);
    }
}