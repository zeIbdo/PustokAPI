using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Contexts;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;
using Pustok.Infrastructure.Repositories.Implementations.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pustok.Infrastructure.Repositories.Implementations;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<Product> _table;
    public ProductRepository(AppDbContext context) : base(context)
    {
        _appDbContext = context;
        _table = _appDbContext.Set<Product>();
    }
    public IQueryable<Product> GetAll(Expression<Func<Product, bool>>? predicate = null, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null, bool enableTracking = false, bool includeDeleted = false)
    {
        var query = _getQueryWithParamters(include, enableTracking);
        if (predicate is not null)
            query = query.Where(predicate);
        if (orderBy is not null)
            query = orderBy(query);
        if (includeDeleted)
            query = query.IgnoreQueryFilters();
        return query;
    }

    public async Task<Paginate<Product>> GetPaginate(Expression<Func<Product, bool>>? predicate = null, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null, Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null, bool enableTracking = false, bool includeDeleted = false, int index = 0, int size = 10)
    {
        var query = _getQueryWithParamters(include, enableTracking);
        if (predicate is not null)
            query = query.Where(predicate);
        if (orderBy is not null)
            query = orderBy(query);
        if (includeDeleted)
            query = query.IgnoreQueryFilters();
        return await query.ToPaginateAsync(index, size);
    }
}

