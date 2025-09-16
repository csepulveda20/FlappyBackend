using Application.Repositories;
using Domain.Entities;
using FlappyBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters
{
    internal class SessionAdapter(FlappyDbContext dbContext, IUnitOfWork unitOfWork) : ISessionRepository
    {
        private readonly FlappyDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Session> AddAsync(Session session)
        {
            //await _unitOfWork.BeginTransaction();
            try
            {
                var entry = await _dbContext.Sessions.AddAsync(session);
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

        public async Task<Session?> GetByIdAsync(Guid sessionId)
        {
            return await _dbContext.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        public async Task<IReadOnlyList<Session>> GetAllAsync()
        {
            return await _dbContext.Sessions.ToListAsync();
        }
    }
}
