namespace DataAccess.Mapping
{
    public sealed class SideMap: IEntityTypeConfiguration<Side>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Side> builder)
        {
            builder.ToTable("Side", "dbo");

            builder.HasKey(t => t.ID);

            builder.Property(t=>t.ID)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(t=>t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(t=>t.Description)
                .IsRequired()
                .HasColumnType("varchar(500)")
                .HasColumnName("Description")
                .HasMaxLength(500);

            builder.Property(t => t.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasColumnType("decimal(17,2)");

            builder.Property(t => t.InStock)
                .IsRequired()
                .HasColumnName("InStock")
                .HasColumnType("bool")
                .HasDefaultValueSql("True");
        }
    }
}
