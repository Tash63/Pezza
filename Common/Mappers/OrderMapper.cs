namespace Common.Mappers;

using Common.Entities;
using Common.Enums;
using Common.Models.Order;
using Common.Models.OrderModel;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Linq;
using System.Runtime.CompilerServices;

public static class OrderMapper
{
    public static OrderModel Map(this Order entity)
        => new()
        {
            Id = entity.Id,
            Status = entity.Status,
            DateCreated = entity.DateCreated,
            User = entity.User.Map(),
            UserEmail = entity.UserEmail,
        };
    public static Order Map(this OrderModel model) =>
        new()
        {
            Id = model.Id,
            Status = model.Status,
            DateCreated = model.DateCreated,
            User = model.User.Map(),
            UserEmail = model.UserEmail,
        };
    public static Order Map(this CreateOrderModel model)
    => new()
    {
        Status = model.Status,
        DateCreated = DateTime.UtcNow,
        UserEmail = model.UserEmail,
    };

    public static OrderStatusModel Mapstatus(this Order entity)
    {
        return new OrderStatusModel
        {
            Id = entity.Id,
            Status = entity.Status,
            CreatedDate= entity.DateCreated.Value,
        };
    }
    public static IEnumerable<OrderModel> Map(this List<Order> entities)
    => entities.Select(x => x.Map());

    public static IEnumerable<Order> Map(this List<OrderModel> models)
        => models.Select(x => x.Map());
    public static IEnumerable<OrderStatusModel> Mapstatus(this List<Order> entities)
        => entities.Select(x => x.Mapstatus());
}