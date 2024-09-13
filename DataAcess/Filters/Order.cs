using Common.Models.Order;

namespace Common.Filters;

public static class OrderFilter
{
    public static IQueryable<CreateOrderModel> FilterByCustomerId(this IQueryable<CreateOrderModel> query, int? customerID)
    {
        if (!customerID.HasValue)
        {
            return query;
        }

        return query.Where(x => x.CustomerId == customerID.Value);
    }
}