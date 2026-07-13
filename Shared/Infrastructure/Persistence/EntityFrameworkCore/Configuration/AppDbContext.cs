using Microsoft.EntityFrameworkCore;
using eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

// TODO exam: add "using eb17953u202421866.<BoundedContext>.Domain.Model.Aggregates;" per bounded context
// TODO exam: add "using eb17953u202421866.<BoundedContext>.Domain.Model.ValueObjects;" as needed

namespace eb17953u202421866.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application-wide EF Core database context. All bounded contexts share this single DbContext
///     (monolith style), each configuring only its own aggregates in OnModelCreating.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    // TODO exam: one DbSet<TAggregate> per aggregate root, e.g.:
    // public DbSet<StartupIncorporation> StartupIncorporations { get; set; }
    // public DbSet<Folder> Folders { get; set; }
    // public DbSet<Space> Spaces { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ---------------------------------------------------------------
        // TODO exam: configure each aggregate here using Fluent API. Pattern to follow:
        //
        // builder.Entity<StartupIncorporation>().HasKey(t => t.Id);
        // builder.Entity<StartupIncorporation>().Property(t => t.Id).ValueGeneratedOnAdd();
        //
        // // Value object stored as a single scalar column (HasConversion):
        // builder.Entity<StartupIncorporation>()
        //     .Property(t => t.IncorporationIdentifier)
        //     .HasConversion(id => id.Value, value => new AtlasIdentifier(value))
        //     .IsRequired();
        //
        // // Value object with multiple fields flattened into columns (ComplexProperty / owned type):
        // builder.Entity<StartupIncorporation>().ComplexProperty(t => t.Period, period =>
        // {
        //     period.IsRequired();
        //     period.Property(p => p.StartDate).HasColumnName("period_start_date");
        //     period.Property(p => p.CompletionDate).HasColumnName("period_completion_date");
        // });
        //
        // // Enum stored as its underlying value:
        // builder.Entity<StartupIncorporation>().Property(t => t.Status).IsRequired();
        // ---------------------------------------------------------------

        builder.UseSnakeCaseNamingConvention();
    }
}
