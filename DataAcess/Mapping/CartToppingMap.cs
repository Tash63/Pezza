using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAcess.Mapping
{
    internal class CartToppingMap:IEntityTypeConfiguration<CartTopping>
    {
        public void Configure(EntityTypeBuilder<CartTopping> builder)
        {
            builder.ToTable("CartTopping", "dbo");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.ToppingId)
                .HasColumnType("int")
                .HasColumnName("ToppingId")
                .IsRequired();

            builder.Property(t => t.CartID)
                .HasColumnType("int")
                .HasColumnName("CartId")
                .IsRequired ();
        }
    }
}
