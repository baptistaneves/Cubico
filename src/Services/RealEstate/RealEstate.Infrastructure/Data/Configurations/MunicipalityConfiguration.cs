namespace RealEstate.Infrastructure.Data.Configurations;

internal class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
{
    public void Configure(EntityTypeBuilder<Municipality> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            provinceId => provinceId.Value,
            dbId => MunicipalityId.Of(dbId));

        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.ToTable("Municipalities");
    }
}