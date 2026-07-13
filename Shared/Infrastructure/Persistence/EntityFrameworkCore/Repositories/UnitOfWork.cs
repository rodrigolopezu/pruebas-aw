using eb17953u202421866.Shared.Domain.Repositories;
using eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core based implementation of <see cref="IUnitOfWork" />.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
