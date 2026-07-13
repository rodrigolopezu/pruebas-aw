using Microsoft.EntityFrameworkCore;
using eb17953u202421866.Shared.Domain.Repositories;
using eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core based implementation of <see cref="IBaseRepository{TEntity}" />.
///     Bounded-context-specific repositories inherit from this to get the common CRUD operations for free,
///     and only add their own query methods (ExistsByXAsync, CountByXAsync, etc.).
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
/// <typeparam name="TEntity">The aggregate root entity type.</typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;

    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }
}
