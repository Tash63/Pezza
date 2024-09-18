﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mapping;

public sealed class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order", "dbo");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName("Id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Status)
            .IsRequired()
            .HasColumnName("Completed")
            .HasConversion<int>();

        builder.HasOne(o => o.Customer)
            .WithOne()
            .HasForeignKey<Order>(o => o.CustomerId);

        // Many-to-many relationship with Pizza
        builder.HasMany(o => o.Pizzas)
            .WithMany()
            .UsingEntity(j => j.ToTable("OrderPizzas"));

        builder.HasMany(o => o.Sides)
            .WithMany()
            .UsingEntity(j => j.ToTable("OrderSides"));

        builder.Property(t => t.DateCreated)
            .IsRequired()
            .HasColumnName("DateCreated")
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");
    }
}