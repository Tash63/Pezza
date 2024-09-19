using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAcess.Mapping
{
    public sealed class ToppingMap :IEntityTypeConfiguration<Topping>
    {
        public void Configure(EntityTypeBuilder<Topping> builder)
        {

            builder.ToTable("Topping", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(17,2)")
                .HasColumnName("Price")
                .IsRequired();

            builder.Property(x=>x.Name)
                .HasColumnName ("Name")
                .HasColumnType ("varchar(100)")
                .IsRequired ();

            builder.Property(x => x.Additional)
                .HasColumnName("Additional")
                .HasColumnType("bool")
                .IsRequired();

            builder.Property(x => x.PizzaId)
                 .HasColumnType("int")
                 .HasColumnName("PizzaId")
                 .IsRequired();
    

        }
    }
}
