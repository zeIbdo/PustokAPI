using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pustok.Domain.Entities.Common;
using Pustok.Infrastructure.Contexts;
using System.Security.Claims;

namespace Pustok.Infrastructure.Interceptors;

public class AuditInterceptor:SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ChangeEntity(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ChangeEntity(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    public void ChangeEntity(DbContext? context)
    {
        if (context == null) return;
        if (context is AppDbContext appDbContext && appDbContext.BypassAuditableInterceptor)
            return;
        var time = DateTimeOffset.UtcNow;
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
        foreach(var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            var existingEntry = context.ChangeTracker.Entries<BaseAuditableEntity>().FirstOrDefault(x => x.Entity.Id == entry.Entity.Id && x != entry);
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                entry.Entity.CreatedBy = userId;
                entry.Entity.UpdatedBy = null;
                entry.Entity.IsDeleted = false;
            }
            else if(entry.State == EntityState.Modified)
            {
                if(existingEntry is not null)
                    context.Entry(existingEntry.Entity).State = EntityState.Detached;
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }
            else if(entry.State == EntityState.Deleted)
            {
                if (existingEntry is not null)
                    context.Entry(existingEntry.Entity).State = EntityState.Detached;
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedBy = userId;
                entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}
