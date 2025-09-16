using Application.Repositories;
using Domain.Entities;
using FlappyBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters
{
    public class ScoreAdapter(FlappyDbContext dbContext, IUnitOfWork unitOfWork) : IScoreRepository
    {
        private readonly FlappyDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Score> AddScoreAsync(Score score)
        {
            //await _unitOfWork.BeginTransaction();
            try
            {
                var entry = await _dbContext.Scores.AddAsync(score);
                //await _unitOfWork.SaveChanges();
                //await _unitOfWork.CommitTransaction();
                return entry.Entity;
            }
            catch
            {
                //await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IReadOnlyList<Score>> GetScoresByAliasAsync(string alias)
        {
            return await _dbContext.Scores
                .Where(s => s.Alias == alias)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Score>> GetTopScoresAsync(int limit = 10)
        {
            return await _dbContext.Scores
                .OrderByDescending(s => s.Points)
                .Take(limit)
                .ToListAsync();
        }
    }
}
