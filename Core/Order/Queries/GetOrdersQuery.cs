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
                if(pizzaentity!=null)
                {
                    orderedpizzas.Add(pizzaentity);
                }
            }
            List<Side> sides=new List<Side>();
            for(int j = 0; j < paged[i].SideIds.Count;j++)
            {
                var sidequery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(s => s.ID == id));
                var sideentity = await sidequery(databaseContext, paged[i].SideIds.ElementAt(j));
                if(sideentity != null)
                {
                    sides.Add(sideentity);
                }
            }
            Order tempOrder = new Order()
            {
                Id = paged[i].Id,
                CustomerId = paged[i].CustomerId,
                Pizzas=orderedpizzas,
                PizzaIds = paged[i].PizzaIds,
                Status = paged[i].Status,
                DateCreated = paged[i].DateCreated,
                Customer = customerentity,
                SideIds = paged[i].SideIds,
                Sides = sides
            };
            orders.Add(tempOrder);
        }
        return ListResult<OrderModel>.Success(orders.Map(), count);
    }
}