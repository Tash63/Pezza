using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAcess.Mapping
{
    class CartMap:IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart", "dbo");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.SideID)
                .HasColumnType("int")
                .HasColumnName("SideID")
                .HasDefaultValue(null);

            builder.Property(t => t.PizzaID)
                .HasColumnType("int")
                .HasColumnName("PizzaID");

            builder.Property(t=>t.UserEmail)
                .HasColumnType("varchar(100)")
                .HasColumnName("UserEmail")
                .IsRequired();
        }
    }
}
