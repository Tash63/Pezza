using Common.Entities;
using Common.Models.Order;
using Common.Models.Side;
using Common.Models.Topping;

namespace Core.Order.Queries
{
    public class SearchOrdersQuery : IRequest<ListResult<OrderModel>>
    {
        public SearchOrderModel? data { get; set; }
    }

    class SearchOrdersHandler(DatabaseContext databaseContext) : IRequestHandler<SearchOrdersQuery, ListResult<OrderModel>>
    {
        public async Task<ListResult<OrderModel>> Handle(SearchOrdersQuery request, CancellationToken cancellationToken)
        {
            var model = request.data;
            if (request.data == null)
            {
                return ListResult<OrderModel>.Failure("Error");
            }

            if (string.IsNullOrEmpty(request.data.OrderBy))
            {
                model.OrderBy = "DateCreated desc";
            }
            var entities = databaseContext.Orders.Select(x => x)
                .FilterByCustomerEmail(model.UserEmail)
                .FilterByDateCreated(model.DateCreated)
                .FilterByStatus(model.Status)
                .FilterByOrderID(model.Id)
                .OrderBy(model.OrderBy);

            var count = entities.Count();
            var paged = await entities.ApplyPaging(model.PagingArgs).ToListAsync();
            // for each order we going to genereate the order model with the sides and the pizzas and customer data
            List<OrderModel> orders = new List<OrderModel>();
            for (int i = 0; i < paged.Count; i++)
            {
                // get the list of pizzas
                List<PizzaModel> pizzas = new List<PizzaModel>();
                List<int> pizzaQuantity = new List<int>();
                List<int> sideQuantity = new List<int>();
                //get side informaiton
                List<SideModel> sides = new List<SideModel>();
                List<List<ToppingModel>> toppings = new List<List<ToppingModel>>();
                var orderentities = databaseContext.OrderPizzas.Select(x => x).AsNoTracking().Where(x => x.OrderId == paged[i].Id).ToList();
                for (int j = 0; j < orderentities.Count(); j++)
                {
                    if (orderentities.ElementAt(j).PizzaId.HasValue)
                    {
                        var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(x => x.Id == id));
                        var pizzaentity = await pizzaquery(databaseContext, orderentities.ElementAt(j).PizzaId.Value);
                        pizzaQuantity.Add(orderentities.ElementAt(j).Quantity);
                        if (pizzaentity != null)
                        {
                            pizzas.Add(pizzaentity.Map());
                            // get toppings for this pizza for the given order
                            var pizzatopping = databaseContext.OrderPizzaToppings
                                .Select(x => x)
                                .Where(x => x.OrderPizzaId == orderentities.ElementAt(j).Id)
                                .AsNoTracking().ToList();
                            List<ToppingModel> pizzatoppings = new List<ToppingModel>();
                            for (int k = 0; k < pizzatopping.Count; k++)
                            {
                                // get the actual toppings from the db
                                var toppingquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Toppings.FirstOrDefault(x => x.Id == id));
                                var toppingresult = await toppingquery(databaseContext, pizzatopping[k].ToppingId);
                                if (toppingresult != null)
                                {
                                    pizzatoppings.Add(new ToppingModel
                                    {
                                        Additional = toppingresult.Additional,
                                        Id = toppingresult.Id,
                                        InStock = toppingresult.InStock,
                                        Price = toppingresult.Price,
                                        Name = toppingresult.Name,
                                        PizzaId = toppingresult.PizzaId,
                                    });
                                }
                            }
                            toppings.Add(pizzatoppings);

                        }
                    }
                    else
                    {
                        var sidequery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(x => x.ID == id));
                        var sideentity = await sidequery(databaseContext, orderentities.ElementAt(j).SideID.Value);
                        sideQuantity.Add(orderentities.ElementAt(j).Quantity);
                        if (sideentity != null)
                        {
                            sides.Add(new SideModel
                            {
                                Description = sideentity.Description,
                                Id = sideentity.ID,
                                InStock = sideentity.InStock,
                                Price = sideentity.Price,
                                Name = sideentity.Name,
                            });
                        }
                    }
                }
                var customerquery = EF.CompileAsyncQuery((DatabaseContext db, string email) => db.Users.FirstOrDefault(x => x.Email == email));
                var customereneity = await customerquery(databaseContext, paged[i].UserEmail);
                orders.Add(new OrderModel
                {
                    User = customereneity.Map(),
                    UserEmail = customereneity.Email,
                    Pizzas = pizzas,
                    Sides = sides,
                    Status = paged.ElementAt(i).Status,
                    DateCreated = paged.ElementAt(i).DateCreated,
                    Id = paged.ElementAt(i).Id,
                    SideQuantity = sideQuantity,
                    PizzaQuantity = pizzaQuantity,
                    Toppings = toppings,
                });
            }
            return ListResult<OrderModel>.Success(orders, count);
        }

    }
}