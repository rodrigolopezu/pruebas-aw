namespace eb17953u202421866.Shared.Domain.Repositories;

/// <summary>
///     Base repository interface with the common CRUD operations every aggregate repository needs.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
/// <typeparam name="TEntity">The aggregate root entity type.</typeparam>
public interface IBaseRepository<TEntity>
{
    /// <summary>Adds an entity to the repository.</summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Finds an entity by its primary key.</summary>
    /// <returns>The entity if found, otherwise null.</returns>
    Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Marks an entity as modified.</summary>
    void Update(TEntity entity);

    /// <summary>Marks an entity for removal.</summary>
    void Remove(TEntity entity);

    /// <summary>Lists all entities of this aggregate type.</summary>
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
}
