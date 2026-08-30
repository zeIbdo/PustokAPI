using Pustok.Domain.Entities;
using Pustok.Infrastructure.Contexts;
using Pustok.Infrastructure.Repositories.Abstractions;
using Pustok.Infrastructure.Repositories.Implementations.Generic;

namespace Pustok.Infrastructure.Repositories.Implementations;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
    }
}
