using Application.Repositories;
using Domain.Entities;

namespace Application.UseCase
{
    public class AliasUseCase(IAliasRepository aliasRepository)
    {
        private readonly IAliasRepository _aliasRepository = aliasRepository;

        public async Task<Alias?> GetByNameAsync(string name)
            => await _aliasRepository.GetByNameAsync(name);

        public async Task<Alias> AddAsync(Alias alias)
            => await _aliasRepository.AddAsync(alias);

        public async Task<IReadOnlyList<Alias>> GetAllAsync()
            => await _aliasRepository.GetAllAsync();
    }
}
