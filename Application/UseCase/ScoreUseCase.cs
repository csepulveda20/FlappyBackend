using Application.Models;
using Application.Repositories;
using Application.UseCase.Dtos;
using Domain.Entities;

namespace Application.UseCase
{
    public class ScoreUseCase(IScoreRepository scoreRepository, IAliasRepository aliasRepository, ISessionRepository sessionRepository) 
    {
        private readonly IScoreRepository _scoreRepository = scoreRepository ?? throw new ArgumentNullException(nameof(scoreRepository));

        private readonly IAliasRepository _aliasRepository = aliasRepository ?? throw new ArgumentNullException(nameof(aliasRepository));

        private readonly ISessionRepository _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));

        // Caso de uso: Registrar puntaje
        public async Task<ApiResponse<Score>> RegisterScoreAsync(ScoreDto score)
        {

            Alias alias = Alias.Create(score.Alias);

            Session session = Session.Create(score.Alias);

            Score createScore = Score.Create(score.Alias, score.Points);

            createScore.SessionId = session.SessionId;

            await _sessionRepository.AddAsync(session);

            Score entity = await _scoreRepository.AddScoreAsync(createScore);

            await _aliasRepository.AddAsync(alias);

            return ApiResponse<Score>.Success(entity, "Score registered successfully");
        }

        // Caso de uso: Obtener Top N puntajes
        public async Task<IReadOnlyList<Score>> GetTopScoresAsync(int limit = 10)
            => await _scoreRepository.GetTopScoresAsync(limit);

        // Caso de uso: Historial de puntajes de un alias
        public async Task<IReadOnlyList<Score>> GetScoresByAliasAsync(string alias)
            => await _scoreRepository.GetScoresByAliasAsync(alias);
    }
}
