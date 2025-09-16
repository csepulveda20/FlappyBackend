using Application.Repositories;
using Domain.Entities;

namespace Application.UseCase
{
    public class SessionUseCase(ISessionRepository sessionRepository)
    {
        private readonly ISessionRepository _sessionRepository = sessionRepository;

        public async Task<Session> AddAsync(Session session)
            => await _sessionRepository.AddAsync(session);

        public async Task<Session?> GetByIdAsync(Guid sessionId)
            => await _sessionRepository.GetByIdAsync(sessionId);

        public async Task<IReadOnlyList<Session>> GetAllAsync()
            => await _sessionRepository.GetAllAsync();
    }
}
