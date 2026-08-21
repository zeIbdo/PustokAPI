using Microsoft.EntityFrameworkCore.Query;
using Pustok.Domain.Entities.Common;
using Pustok.Infrastructure.Paging;
using System.Linq.Expressions;

namespace Pustok.Infrastructure.Repositories.Abstractions.Generic;

public interface IRepositoryAsync<T> where T : BaseEntity
{
    Task<T?> GetAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false);
    IQueryable<T> GetAll(Expression<Func<IQueryable<T>, bool>>? predicate=null,
        Func<IQueryable<T>,IIncludableQueryable<T,object>>? include = null,
        Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy=null, bool enableTracking = false);
    Task<Paginate<T>> GetPaginate(Expression<Func<IQueryable<T>, bool>>? predicate=null,
        Func<IQueryable<T>,IIncludableQueryable<T,object>>? include = null,
        Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy=null, bool enableTracking = false);

    Task<bool> DoesExistAsync(Expression<Func<T, bool>> expression);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<int> SaveChangesAsync(bool bypassInterceptor = false);
}
