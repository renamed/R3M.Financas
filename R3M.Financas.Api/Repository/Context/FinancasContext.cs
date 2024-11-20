using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository.Context;

public class FinancasContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<Movimentation> Movimentations { get; set; }

    public FinancasContext(DbContextOptions<FinancasContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigCategories(modelBuilder);
        ConfigPeriods(modelBuilder);
        ConfigInstitutions(modelBuilder);
        ConfigMovimentations(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigMovimentations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movimentation>(e =>
        {
            ConfigRegistry(e);
            
            e.Property(p => p.Description)
                .HasMaxLength(30)
                .IsRequired();

            e.HasOne(k => k.Period)
                .WithMany()
                .HasForeignKey(p => p.PeriodId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(k => k.Institution)
                .WithMany()
                .HasForeignKey(p => p.InstitutionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(k => k.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigInstitutions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Institution>(e =>
        {
            ConfigRegistry(e);

            e.Property(p => p.Name)
                .HasMaxLength(20)
                .IsRequired();

            e.HasIndex(p => p.Name)
                .IsUnique();
        });
    }

    private void ConfigCategories(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            ConfigRegistry(e);

            e.Property(p => p.Name)
                .HasMaxLength(20)
                .IsRequired();

            e.HasIndex(p => p.Name)
                .IsUnique();

            e.HasOne(o => o.Parent)
                .WithMany()
                .HasForeignKey(o => o.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigPeriods(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Period>(e =>
        {
            ConfigRegistry(e);

            e.Property(p => p.Description)
                .HasMaxLength(5)
                .IsFixedLength()
                .IsRequired();

            e.HasIndex(p => p.Description)
                .IsUnique();
        });
    }

    private void ConfigRegistry<T>(EntityTypeBuilder<T> builder)
        where T : Register
    {
        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }


    public override int SaveChanges()
    {
        ConfigAddDate();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ConfigAddDate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ConfigAddDate();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConfigAddDate();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ConfigAddDate()
    {
        foreach(var e in ChangeTracker.Entries().Where(x => x.State == EntityState.Added))
        {
            ((Register)e.Entity).InsertDate = DateTime.UtcNow;
        }
    }
}
