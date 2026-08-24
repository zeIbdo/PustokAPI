using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pustok.Domain.Entities.Common;
using Pustok.Infrastructure.Contexts;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions.Generic;
using System.Linq.Expressions;

namespace Pustok.Infrastructure.Repositories.Implementations.Generic;

public class Repository<T> : IRepositoryAsync<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _table;

    public Repository(AppDbContext context)
    {
        _context = context;
        _table = _context.Set<T>();
    }

    public async Task<T> CreateAsync(T entity)
    {
        var entityEntry = await _table.AddAsync(entity);
        return entityEntry.Entity;
    }

    public T Delete(T entity)
    {
        var entityEntry = _table.Remove(entity);

        return entityEntry.Entity;
    }

    public async Task<bool> DoesExistAsync(Expression<Func<T, bool>> expression)
    {
        return await _table.AnyAsync(expression);
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
    {
        var query =  _getQueryWithParamters(include, enableTracking);
        if (predicate is not null)
            query = query.Where(predicate);
        if (orderBy is not null)
            query = orderBy(query);
        return query;
    }


    public async Task<T?> GetAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
    {
        var query = _getQueryWithParamters(include,enableTracking);
        return await query.FirstOrDefaultAsync(x=>x.Id==id);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
    {
        var query = _getQueryWithParamters(include, enableTracking);
        return await query.FirstOrDefaultAsync(expression);
    }

    public Task<Paginate<T>> GetPaginateAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false,int index = 0,int size = 10)
    {
        var query = _getQueryWithParamters(include, enableTracking);
        if(predicate is not null)
            query = query.Where(predicate);
        if (orderBy is not null)
            query = orderBy(query);
        return query.ToPaginateAsync(index, size);
    }

    public Task<int> SaveChangesAsync(bool bypassInterceptor = false)
    {
        throw new NotImplementedException();
    }

    public T Update(T entity)
    {
        var entityEntry = _table.Update(entity);
        return entityEntry.Entity;
    }
    protected IQueryable<T> _getQueryWithParamters(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include, bool enableTracking)
    {
        var query = _table.AsQueryable();
        if (include is not null)
            query = include(query);
        if (!enableTracking)
            query = query.AsNoTracking();
        return query;
    }
}
