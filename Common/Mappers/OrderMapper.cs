namespace Common.Mappers;

using Common.Entities;
using Common.Models.Customer;
using Common.Models.Order;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Linq;
using System.Runtime.CompilerServices;

public static class OrderMapper
{
    public static OrderModel Map(this Order entity)
        => new()
        { Id = entity.Id,
          Completed=entity.Completed,
        CustomerId=entity.CustomerId,
        Customer=entity.Customer.Map(),
        PizzaIds=entity.PizzaIds,
        Pizzas=entity.Pizzas.ToList().Map(),
        DateCreated=entity.DateCreated,
        Sides=entity.Sides.ToList().Map(),
        SideIds=entity.SideIds
        };
    public static Order Map(this OrderModel model) =>
        new()
        {
            Id = model.Id,
            Completed = model.Completed,
            CustomerId = model.CustomerId,
            Customer = model.Customer.Map(),
            PizzaIds = model.PizzaIds,
            Pizzas = model.Pizzas.Map(),
            DateCreated = model.DateCreated,
            SideIds=model.SideIds,
            Sides=model.Sides.ToList().Map(),
        };
    public static Order Map(this CreateOrderModel model)
    => new()
    {
        Completed = false,
        CustomerId = model.CustomerId,
        PizzaIds = model.PizzaIds,
        DateCreated = DateTime.UtcNow,
        SideIds = model.SideIds,
    };
    public static IEnumerable<OrderModel> Map(this List<Order> entities)
    => entities.Select(x => x.Map());

    public static IEnumerable<Order> Map(this List<OrderModel> models)
        => models.Select(x => x.Map());
}