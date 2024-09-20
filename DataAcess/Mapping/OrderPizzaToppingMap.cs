namespace DataAcess.Mapping
{
    public class OrderPizzaToppingMap: IEntityTypeConfiguration<OrderPizzaTopping>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderPizzaTopping> builder)
        {
            builder.ToTable("OrderPizzaTopping", "dbo");

            builder.HasKey(e=>e.Id);

            builder.Property(e=>e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("int");

            builder.Property(e=>e.OrderPizzaId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("OrderPizzaId");
            builder.Property(e => e.ToppingId)
                .IsRequired()
                .HasColumnName("ToppingId")
                .HasColumnType("int");

        }
    }
}
