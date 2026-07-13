namespace eb17953u202421866.Shared.Domain.Repositories;

/// <summary>
///     Unit of Work abstraction: commits all pending changes tracked by the DbContext in one transaction.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>Persists all pending changes to the database.</summary>
    Task CompleteAsync(CancellationToken cancellationToken = default);
}
