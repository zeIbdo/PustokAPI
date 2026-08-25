using Pustok.Domain.Entities;
using Pustok.Infrastructure.Repositories.Abstractions.Generic;

namespace Pustok.Infrastructure.Repositories.Abstractions;

public interface ICategoryRepository : IRepositoryAsync<Category>
{
    Task<List<Category>> GetAllDescendantsAsync(int categoryId);
}
