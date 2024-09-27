using Common.Enums;

namespace Common.Filters;

public static class OrderFilter
{
    public static IQueryable<Order> FilterByCustomerEmail(this IQueryable<Order> query, string? CustomerEmail)
    {
        if (string.IsNullOrEmpty(CustomerEmail))
        {
            return query;
        }

        return query.Where(x => x.UserEmail == CustomerEmail);
    }

    public static IQueryable<Order> FilterByStatus(this IQueryable<Order> query,OrderStatus? status)
    {
        if (!status.HasValue)
        {
            return query;
        }
        return query.Where(x => x.Status == status.Value);
    }

    public static IQueryable<Order> FilterByDateCreated(this IQueryable<Order> query,DateTime? datecreated)
    {
        if (!datecreated.HasValue)
        {
            return query;            
        }
        return query.Where(x => x.DateCreated.Value.Date == datecreated.Value.Date);
    }

    public static IQueryable<Order>FilterByOrderID(this IQueryable<Order> query,int? orderID)
    {
        if(!orderID.HasValue)
        {
            return query;
        }
        return query.Where(x => x.Id == orderID.Value);
    }
}