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

        // get customer details
        Customer customer;
        var customerquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
        var customerentity = await customerquery(databaseContext, paged.ElementAt(0).CustomerId);
        List<OrderModel> orders=new List<OrderModel>();

        // genereate order
        for(int i=0;i<paged.Count;i++)
        {
            // First loop is for a given order
            // this is for each order for this customer so I will filtter the orderPizza table
            // to get the pizzaids and the orderPizzaIDS wich i will use in the topping table\
            List<Pizza> pizzas = new List<Pizza>();
            List<List<Topping>> AllToppings = new List<List<Topping>>();
            var orderentities = databaseContext.OrderPizzas.Select(x => x).AsNoTracking().Where(x => x.OrderId == paged[i].Id);
            if(orderentities!=null)
            {
                List<OrderPizza> orderpizza=orderentities.ToList();
                //get the pizzas for the given order
                for(int j=0;j<orderpizza.Count;j++)
                {
                    var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db,int id)=>db.Pizzas.FirstOrDefault(x=>x.Id==id));
                    var result = await pizzaquery(databaseContext, orderpizza[j].PizzaId);
                    if(result!=null)
                    {
                        // add pizza and look for the toppings in the orderpizza topping table filter by orderpizzaid
                        pizzas.Add(result);
                        var toppingentities = databaseContext.OrderPizzaToppings.Select(x => x).AsNoTracking().Where(x => x.OrderPizzaId == orderpizza[j].Id);
                        List<Topping> PizzaToppings = new List<Topping>();
                        List<OrderPizzaTopping> toppings=toppingentities.ToList();
                        //get the toppings for this pizza
                        for(int k=0;k<toppings.Count;k++)
                        {
                            // get the toppings and add to the topping list for this pizza
                            var toppingquery=EF.CompileAsyncQuery((DatabaseContext db,int id)=>db.Toppings.FirstOrDefault(x=>x.Id==id));
                            var toppingresults = await toppingquery(databaseContext, toppings[k].ToppingId);
                            PizzaToppings.Add(toppingresults);
                        }
                        AllToppings.Add(PizzaToppings);
                    }
                }
            }

            // get the sides associated with this order
            List<Side> sides = new List<Side>();
            for (int j = 0; j < paged[i].SideIds.Count; j++)
            {
                var sidequery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(s => s.ID == id));
                var sideentity = await sidequery(databaseContext, paged[i].SideIds.ElementAt(j));
                if (sideentity != null)
                {
                    sides.Add(sideentity);
                }
            }

            OrderModel tempOrder = new OrderModel()
            {
                Id = paged[i].Id,
                CustomerId = paged[i].CustomerId,
                Pizzas = pizzas.Map(),
                Status = paged[i].Status,
                DateCreated = paged[i].DateCreated,
                Customer = customerentity.Map(),
                Sides = sides.Map(),
                Toppings = AllToppings.Map(),
            };
            orders.Add(tempOrder);
        }

        /*for (int i = 0;i<paged.Count;i++)
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
        }*/
        return ListResult<OrderModel>.Success(orders, count);
    }
}