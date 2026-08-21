using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace Pustok.Infrastructure.Paging;

public static class PaginateIQueryableExtensions
{
    public static async Task<Paginate<T>> ToPaginateAsync<T>(this IQueryable<T> query, int index, int size)
    {
        int count = await query.CountAsync();
        int pages = (int)Math.Ceiling(count / (double)size);
        var items = await query.Skip(index*size).Take(size).ToListAsync();
        return new Paginate<T>
        {
            Index = index,
            Size = size,
            Count = count,
            Pages = pages,
            Items = items
        };
    }
}
