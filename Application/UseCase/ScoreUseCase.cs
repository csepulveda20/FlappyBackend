using Application.Models;
using Application.Repositories;
using Application.UseCase.Dtos;
using Domain.Entities;

namespace Application.UseCase
{
    public class ScoreUseCase(IScoreRepository scoreRepository, IAliasRepository aliasRepository, ISessionRepository sessionRepository, IUnitOfWork unitOfWork) 
    {
        private readonly IScoreRepository _scoreRepository = scoreRepository ?? throw new ArgumentNullException(nameof(scoreRepository));

        private readonly IAliasRepository _aliasRepository = aliasRepository ?? throw new ArgumentNullException(nameof(aliasRepository));

        private readonly ISessionRepository _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));

        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        // Caso de uso: Registrar puntaje
        public async Task<ApiResponse<Score>> RegisterScoreAsync(ScoreDto score)
        {
            Alias? alias = null;

            if (!await _aliasRepository.ValidateByName(score.Alias))
            {
                alias = Alias.Create(score.Alias);
            }

            await _unitOfWork.BeginTransaction();

            try
            {
                Session session = Session.Create(score.Alias);

                Score createScore = Score.Create(score.Alias, score.Points);

                createScore.SessionId = session.SessionId;

                if (alias != null)
                {
                    await _aliasRepository.AddAsync(alias);                
                }

                await _sessionRepository.AddAsync(session);

                Score entity = await _scoreRepository.AddScoreAsync(createScore);

                await _unitOfWork.CommitTransaction();
                await _unitOfWork.SaveChanges();

                return ApiResponse<Score>.Success(entity, "Score registered successfully");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();

                return ApiResponse<Score>.Failure("ERROR, Registro de score fallido.");
            }
        }

        // Caso de uso: Obtener Top N puntajes
        public async Task<IReadOnlyList<Score>> GetTopScoresAsync(int limit = 10)
            => await _scoreRepository.GetTopScoresAsync(limit);

        // Caso de uso: Historial de puntajes de un alias
        public async Task<IReadOnlyList<Score>> GetScoresByAliasAsync(string alias)
            => await _scoreRepository.GetScoresByAliasAsync(alias);
    }
}
