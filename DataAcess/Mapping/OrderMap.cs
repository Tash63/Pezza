﻿namespace Common.Mappers;

using Common.Models.Customer;
using Common.Models.Order;
using System.Linq;

public static class OrderMapper
{
    public static OrderModel Map(this Order entity)
        => new()
        {
            Id = entity.Id,
            Completed = entity.Completed,
            CustomerId = entity.CustomerId,
            Customer = entity.Customer.Map(),
            PizzaIds = entity.PizzaIds,
            Pizzas = entity.Pizzas.Map(),
            DateCreated = entity.DateCreated
        };

    public static Order Map(this OrderModel model)
        => new()
        {
            Id = model.Id,
            Completed = model.Completed,
            CustomerId = model.CustomerId,
            Customer = model.Customer.Map(),
            PizzaIds = model.PizzaIds,
            Pizzas = model.Pizzas.Map(),
            DateCreated = model.DateCreated
        };

    public static Order Map(this CreateOrderModel model)
        => new()
        {
            Completed = false,
            CustomerId = model.CustomerId,
            PizzaIds = model.PizzaIds,
            DateCreated = DateTime.UtcNow
        };

    public static IEnumerable<CustomerModel> Map(this List<Customer> entities)
        => entities.Select(x => x.Map());

    public static IEnumerable<Customer> Map(this List<CustomerModel> models)
        => models.Select(x => x.Map());
}