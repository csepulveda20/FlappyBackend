using Application.Repositories;
using FlappyBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{
    public class UnitOfWork(FlappyDbContext context) : IUnitOfWork
    {
        private readonly FlappyDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private IDbContextTransaction? _transaction;

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                return;
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                return;
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransaction(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                return;
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            await _context.DisposeAsync();
        }
    }
}
