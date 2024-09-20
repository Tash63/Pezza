using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Mapping
{
   public  class OrderPizzaMap: IEntityTypeConfiguration<OrderPizza>
    {
        public void Configure(EntityTypeBuilder<OrderPizza> builder)
        {
            builder.ToTable("OrderPizza", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.OrderId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("OrderId");

            builder.Property(x => x.PizzaId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("PizzaId");
        }
    }
}
