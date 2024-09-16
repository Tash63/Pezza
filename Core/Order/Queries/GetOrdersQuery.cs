namespace Core.Order.Queries;

using Common.Models.Order;
using System.Linq;
using Common.Entities;
public class GetOrdersQuery : IRequest<ListResult<OrderModel>>
{
    public int CustomerID { get; set; }
}

public class GetOrdersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetOrdersQuery, ListResult<OrderModel>>
{
    public async Task<ListResult<OrderModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var entities = databaseContext.Orders
            .Select(x => x)
            .AsNoTracking()
            .FilterByCustomerId(request.CustomerID)
            .OrderBy("DateCreated desc");
        var count = entities.Count();
        var paged = await entities.ToListAsync(cancellationToken);
        //get customer details
        Customer customer;
        var customerquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
        var customerentity = await customerquery(databaseContext, paged.ElementAt(0).CustomerId);
        List<Order> orders=new List<Order>();
        for (int i = 0;i<paged.Count;i++)
        {
            //get pizzas
            List<Pizza> orderedpizzas= new List<Pizza>();
            for (int j = 0; j < paged[i].PizzaIds.Count; j++)
            {
                var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
                var pizzaentity = await pizzaquery(databaseContext, paged[i].PizzaIds[j]);
                orderedpizzas.Add(new Pizza()
                {
                    Id=pizzaentity.Id,
                    Name=pizzaentity.Name,
                    DateCreated=pizzaentity.DateCreated,
                    Description=pizzaentity.Description,
                    Price=pizzaentity.Price,
                });
            }
            Order tempOrder = new Order()
            {
                Id = paged[i].Id,
                CustomerId = paged[i].CustomerId,
                PizzaIds = paged[i].PizzaIds,
                Completed = paged[i].Completed,
                DateCreated = paged[i].DateCreated,
                Customer = customerentity,
            };
            orders.Add(tempOrder);
        }
        return ListResult<OrderModel>.Success(orders.Map(), count);
    }
}