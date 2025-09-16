using Domain.Entities;

namespace Application.Repositories
{
    public interface ISessionRepository
    {
        Task<Session> AddAsync(Session session);
        Task<Session?> GetByIdAsync(Guid sessionId);
        Task<IReadOnlyList<Session>> GetAllAsync();
    }
}
