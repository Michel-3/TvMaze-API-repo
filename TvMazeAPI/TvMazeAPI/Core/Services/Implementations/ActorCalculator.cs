using TvMazeAPI.Core.Services.Interfaces;
using TvMazeAPI.Repository;

namespace TvMazeAPI.Core.Services.Implementations
{
    public class ActorCalculator : IActorCalculator
    {
        private readonly ActorRepository _actorRepository;

        public ActorCalculator(ActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public async Task<IEnumerable<object>> CalculateActorPercentagesAsync(int month, int year)
        {
            var totalActors = await _actorRepository.GetTotalActors(month, year);

            var actorPercentage = await _actorRepository.CalculatePercentage(month, year, totalActors);

            return actorPercentage;
        }
    }
}
