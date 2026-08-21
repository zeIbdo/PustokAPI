using Microsoft.EntityFrameworkCore.Query;
using Pustok.Domain.Entities.Common;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions.Generic;
using System.Linq.Expressions;

namespace Pustok.Infrastructure.Repositories.Implementations.Generic;

public class Repository<T> : IRepositoryAsync<T> where T : BaseEntity
{

    public Task<T> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DoesExistAsync(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> GetAll(Expression<Func<IQueryable<T>, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<T>> GetPaginate(Expression<Func<IQueryable<T>, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
