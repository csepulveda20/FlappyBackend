using Domain.Entities;

namespace Application.Repositories
{
    public interface IAliasRepository
    {
        Task<Alias?> GetByNameAsync(string name);
        Task<Alias> AddAsync(Alias alias);
        Task<IReadOnlyList<Alias>> GetAllAsync();
    }
}
