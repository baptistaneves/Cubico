namespace RealEstate.Infrastructure.Data.Configurations;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            provinceId => provinceId.Value,
            dbId => ProvinceId.Of(dbId));

        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.ToTable("Provinces");
    }
}
