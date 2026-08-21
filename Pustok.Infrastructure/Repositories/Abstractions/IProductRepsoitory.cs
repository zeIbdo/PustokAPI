using Microsoft.EntityFrameworkCore.Query;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions.Generic;
using System.Linq.Expressions;

namespace Pustok.Infrastructure.Repositories.Abstractions;

internal interface IProductRepsoitory:IRepositoryAsync<Product>
{
    IQueryable<Product> GetAll(Expression<Func<IQueryable<Product>, bool>>? predicate = null,
        Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null, bool enableTracking = false,bool includeDeleted=false);
    Task<Paginate<Product>> GetPaginate(Expression<Func<IQueryable<Product>, bool>>? predicate = null,
        Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null, bool enableTracking = false, bool includeDeleted = false);
}
