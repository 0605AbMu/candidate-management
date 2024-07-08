using CM.WebApi.Models;
using CM.WebApi.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace CM.WebApi.Brokers;

public class AppDbContext : DbContext
{
    public DbSet<Candidate> Candidates { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Configurations Auditable model based

        var auditableBaseModels = modelBuilder
            .Model
            .GetEntityTypes()
            .Where(x => x.ClrType.BaseType is not null && x.ClrType.BaseType == typeof(AuditableModelBase));

        foreach (var model in auditableBaseModels)
        {
            modelBuilder
                .Entity(model.ClrType)
                .Property(nameof(AuditableModelBase.CreatedAt)).HasDefaultValueSql("current_timestamp")
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder
                .Entity(model.ClrType)
                .Property(nameof(AuditableModelBase.UpdatedAt)).HasDefaultValueSql("current_timestamp")
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();
        }

        #endregion
    }
}
