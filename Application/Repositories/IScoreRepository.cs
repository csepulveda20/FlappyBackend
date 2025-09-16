using Domain.Entities;

namespace Application.Repositories
{
    public interface IScoreRepository
    {
        Task<Score> AddScoreAsync(Score score);
        Task<IReadOnlyList<Score>> GetTopScoresAsync(int limit = 10);
        Task<IReadOnlyList<Score>> GetScoresByAliasAsync(string alias);
    }
}
