namespace RealEstate.Application.Data;

public interface IApplicationDbContext
{
    public DbSet<Province> Provinces { get; }
    public DbSet<Municipality> Municipalities { get; }
    public DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}