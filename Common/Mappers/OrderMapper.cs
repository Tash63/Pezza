namespace Common.Mappers;

using Common.Entities;
using Common.Enums;
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
         Status=entity.Status,
        CustomerId=entity.CustomerId,
        Customer=entity.Customer.Map(),
        DateCreated=entity.DateCreated,
        Sides=entity.Sides.ToList().Map(),
        };
    public static Order Map(this OrderModel model) =>
        new()
        {
            Id = model.Id,
            Status = model.Status,
            CustomerId = model.CustomerId,
            Customer = model.Customer.Map(),
            DateCreated = model.DateCreated,
            Sides=model.Sides.ToList().Map(),
        };
    public static Order Map(this CreateOrderModel model)
    => new()
    {
        Status=model.Status,
        CustomerId = model.CustomerId,
        DateCreated = DateTime.UtcNow,
        SideIds = model.SideIds,
    };
    public static IEnumerable<OrderModel> Map(this List<Order> entities)
    => entities.Select(x => x.Map());

    public static IEnumerable<Order> Map(this List<OrderModel> models)
        => models.Select(x => x.Map());
}