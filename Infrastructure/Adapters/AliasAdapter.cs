using Application.Repositories;
using Domain.Entities;
using FlappyBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters
{
    internal class AliasAdapter(FlappyDbContext dbContext, IUnitOfWork unitOfWork) : IAliasRepository
    {
        private readonly FlappyDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Alias?> GetByNameAsync(string name)
        {
            return await _dbContext.Aliases.FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<Alias> AddAsync(Alias alias)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var entry = await _dbContext.Aliases.AddAsync(alias);
                await _unitOfWork.SaveChanges();
                await _unitOfWork.CommitTransaction();
                return entry.Entity;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IReadOnlyList<Alias>> GetAllAsync()
        {
            return await _dbContext.Aliases.ToListAsync();
        }
    }
}
