namespace Core.Order.Queries;

using Common.Models.Order;
using System.Linq;
using Common.Entities;
public class GetOrdersQuery : IRequest<ListResult<OrderModel>>
{
    public string CustomerEmail { get; set; }
}

public class GetOrdersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetOrdersQuery, ListResult<OrderModel>>
{
    public async Task<ListResult<OrderModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var entities = databaseContext.Orders
            .Select(x => x)
            .AsNoTracking()
            .FilterByCustomerEmail(request.CustomerEmail)
            .OrderBy("DateCreated desc");
        var count = entities.Count();
        var paged = await entities.ToListAsync(cancellationToken);

       
        var customerquery = EF.CompileAsyncQuery((DatabaseContext db, string email) => db.Users.FirstOrDefault(c => c.Email == email));
        var customerentity = await customerquery(databaseContext, paged.ElementAt(0).UserEmail);
        List<OrderModel> orders=new List<OrderModel>();

        // genereate order
        for(int i=0;i<paged.Count;i++)
        {
            // First loop is for a given order
            // this is for each order for this customer so I will filtter the orderPizza table
            // to get the pizzaids and the orderPizzaIDS wich i will use in the topping table\
            List<Pizza> pizzas = new List<Pizza>();
            List<Side> sides = new List<Side>();
            List<List<Topping>> AllToppings = new List<List<Topping>>();
            List<int> sidequantity = new List<int>();
            List<int> pizzaquantity = new List<int>();
            var orderentities = databaseContext.OrderPizzas.Select(x => x).AsNoTracking().Where(x => x.OrderId == paged[i].Id);
            if(orderentities!=null)
            {
                List<OrderPizza> orderpizza=orderentities.ToList();
                //get the pizzas for the given order
                for(int j=0;j<orderpizza.Count;j++)
                {
                    if (orderpizza[j].PizzaId.HasValue)
                    {
                        var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(x => x.Id == id));
                        var result = await pizzaquery(databaseContext, orderpizza[j].PizzaId.Value);
                        pizzaquantity.Add(orderpizza[j].Quantity);
                        if (result != null)
                        {
                            // add pizza and look for the toppings in the orderpizza topping table filter by orderpizzaid
                            pizzas.Add(result);
                            var toppingentities = databaseContext.OrderPizzaToppings.Select(x => x).AsNoTracking().Where(x => x.OrderPizzaId == orderpizza[j].Id);
                            List<Topping> PizzaToppings = new List<Topping>();
                            List<OrderPizzaTopping> toppings = toppingentities.ToList();
                            //get the toppings for this pizza
                            for (int k = 0; k < toppings.Count; k++)
                            {
                                // get the toppings and add to the topping list for this pizza
                                var toppingquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Toppings.FirstOrDefault(x => x.Id == id));
                                var toppingresults = await toppingquery(databaseContext, toppings[k].ToppingId);
                                PizzaToppings.Add(toppingresults);
                            }
                            AllToppings.Add(PizzaToppings);
                        }
                        else
                        {
                            var sidequery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(s => s.ID == id));
                            var sideentity = await sidequery(databaseContext, orderpizza[j].SideID.Value);
                            sidequantity.Add(orderpizza[j].Quantity);
                            if (sideentity != null)
                            {
                                sides.Add(sideentity);
                            }
                        }
                    }
                }
            }

            OrderModel tempOrder = new OrderModel()
            {
                Id = paged[i].Id,
                UserEmail = paged[i].UserEmail,
                Pizzas = pizzas.Map(),
                Status = paged[i].Status,
                DateCreated = paged[i].DateCreated,
                User = customerentity.Map(),
                Sides = sides.Map(),
                Toppings = AllToppings.Map(),
                PizzaQuantity=pizzaquantity,
                SideQuantity=sidequantity
            };
            orders.Add(tempOrder);
        }
        return ListResult<OrderModel>.Success(orders, count);
    }
}