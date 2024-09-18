﻿using Common.Mappers;

namespace Common.Entities;

public class Order
{
    public Order() => this.Pizzas = new HashSet<Pizza>();

    public int Id { get; set; }

    public required int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }

    public DateTime? DateCreated { get; set; }

    public required bool Completed { get; set; }

    public List<int> PizzaIds { get; set; } // List of Pizza IDs

    public List<int> SideIds { get; set; }

    public ICollection<Pizza> Pizzas { get; set; }

    public ICollection<Side> Sides { get; set; }    


}