using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using eb17953u202421866.Shared.Domain.Model.Entities;

namespace eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

/// <summary>
///     EF Core interceptor that automatically populates audit timestamps on any entity
///     that implements <see cref="IAuditableEntity" />.
/// </summary>
/// <remarks>
///     - CreatedAt: set once, only when the entity is first added.
///     - UpdatedAt: refreshed on every add or update.
///     Register it in <c>AppDbContext.OnConfiguring</c> so it applies to every bounded context
///     sharing the same DbContext.
///
///     NOTE ABOUT THE "EntityFrameworkCore.CreatedUpdatedDate" NUGET PACKAGE:
///     Some exam statements (e.g. the ClickUp final exam) explicitly name that package as a
///     technical constraint. Its real public API (verified on nuget.org) requires implementing
///     `IEntityWithCreatedUpdatedDate` with properties named exactly `CreatedDate` / `UpdatedDate` —
///     NOT `CreatedAt` / `UpdatedAt`, which is what most of these exam statements ask you to expose.
///     If you must use that literal package to satisfy a technical constraint, keep your public
///     properties named CreatedAt/UpdatedAt and implement the package's interface EXPLICITLY, e.g.:
///
///     public class MyAggregate : IAuditableEntity, IEntityWithCreatedUpdatedDate
///     {
///         public DateTimeOffset? CreatedAt { get; set; }
///         public DateTimeOffset? UpdatedAt { get; set; }
///
///         DateTimeOffset? IEntityWithCreatedUpdatedDate.CreatedDate
///         {
///             get =&gt; CreatedAt;
///             set =&gt; CreatedAt = value;
///         }
///         DateTimeOffset? IEntityWithCreatedUpdatedDate.UpdatedDate
///         {
///             get =&gt; UpdatedAt;
///             set =&gt; UpdatedAt = value;
///         }
///     }
///
///     ...and register `optionsBuilder.AddCreatedUpdatedInterceptor();` in OnConfiguring instead of
///     this class. Otherwise, this custom interceptor is simpler, has zero extra dependencies, and
///     gives you full control over naming — recommended as the default unless a constraint literally
///     names the package.
///     Author: __YOUR_NAME__
/// </remarks>
public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAuditTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditTimestamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplyAuditTimestamps(DbContext? context)
    {
        if (context is null) return;

        var now = DateTimeOffset.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified) entry.Entity.UpdatedAt = now;
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt ??= now;
        }
    }
}
