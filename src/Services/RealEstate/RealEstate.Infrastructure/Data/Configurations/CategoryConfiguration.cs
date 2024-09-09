namespace RealEstate.Infrastructure.Data.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            provinceId => provinceId.Value,
            dbId => CategoryId.Of(dbId));

        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.ToTable("Categories");
    }
}