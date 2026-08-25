using Microsoft.EntityFrameworkCore;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Contexts;
using Pustok.Infrastructure.Repositories.Abstractions;
using Pustok.Infrastructure.Repositories.Implementations.Generic;

namespace Pustok.Infrastructure.Repositories.Implementations;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly AppDbContext _appDbContext;
    public CategoryRepository(AppDbContext context):base(context)
    {
        _appDbContext = context;
    }

    public async Task<List<Category>> GetAllDescendantsAsync(int categoryId)
    {
        var descendants = new List<Category>();
        var children = await _appDbContext.Categories.Where(x => x.ParentId == categoryId).ToListAsync();
        foreach(var child in children)
        {
            descendants.Add(child);
            var childDescendants = await GetAllDescendantsAsync(child.Id);
            descendants.AddRange(childDescendants);
        }
        return descendants;
    }
}

