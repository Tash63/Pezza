namespace Core.Order.Queries;

using Common.Models.Order;
using System.Linq;

public class GetOrdersQuery : IRequest<ListResult<CreateOrderModel>>
{
    public int CustomerID { get; set; }
}

public class GetOrdersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetOrdersQuery, ListResult<CreateOrderModel>>
{
    public async Task<ListResult<CreateOrderModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var entities = databaseContext.Orders
            .Select(x => x)
            .AsNoTracking()
            .FilterByCustomerId(request.CustomerID)
            .OrderBy("DateCreated desc");
        List<CreateOrderModel> result=entities.ToList();
        var count = entities.Count();
        var paged = await entities.ToListAsync(cancellationToken);
        return ListResult<CreateOrderModel>.Success(paged, count);
    }
}