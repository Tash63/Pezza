using Common.Models.Order;
using Common.Models.Side;

namespace Core.Order.Queries
{
    public class SearchOrdersQuery : IRequest<ListResult<OrderModel>>
    {
        public SearchOrderModel? data { get; set; }
    }

    class SearchOrdersHandler(DatabaseContext databaseContext) : IRequestHandler<SearchOrdersQuery,ListResult<OrderModel>>
    {
        public async Task<ListResult<OrderModel>> Handle(SearchOrdersQuery request,CancellationToken cancellationToken)
        {
            var model=request.data;
            if(request.data==null)
            {
                return ListResult<OrderModel>.Failure("Error");
            }

            if(string.IsNullOrEmpty(request.data.OrderBy))
            {
                model.OrderBy = "DateCreated desc";
            }
            var entities = databaseContext.Orders
                .FilterByCustomerId(model.CustomerId)
                .FilterByDateCreated(model.DateCreated)
                .FilterByStatus(model.Status)
                .OrderBy(model.OrderBy);

            var count=entities.Count();
            var paged=await entities.ApplyPaging(model.PagingArgs).ToListAsync();
            // for each order we going to genereate the order model with the sides and the pizzas and customer data
            List<OrderModel> orders=new List<OrderModel>();
            for(int i=0;i<paged.Count;i++)
            {
                // get the list of pizzas
                List<PizzaModel> pizzas=new List<PizzaModel>();
                for(int j=0;j<paged.ElementAt(i).PizzaIds.Count;j++)
                {
                    var pizzaquery= EF.CompileAsyncQuery((DatabaseContext db,int id)=>db.Pizzas.FirstOrDefault(x=>x.Id==id));
                    var pizzaentity = await pizzaquery(databaseContext, paged.ElementAt(i).PizzaIds.ElementAt(j));
                    if(pizzaentity!=null)
                    {
                        pizzas.Add(new PizzaModel
                        {
                            Category=pizzaentity.Category,
                            Name=pizzaentity.Name,
                            DateCreated=pizzaentity.DateCreated,
                            Description=pizzaentity.Description,
                            Id=pizzaentity.Id,
                            InStock=pizzaentity.InStock,
                            Price=pizzaentity.Price,
                        });
                    }
                }
                //get side informaiton
                List<SideModel> sides=new List<SideModel>();
                for(int j=0;j<paged.ElementAt(i).SideIds.Count;j++)
                {
                    var sidequery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(x => x.ID == id));
                    var sideentity = await sidequery(databaseContext, paged.ElementAt(i).SideIds.ElementAt(j));
                    if(sideentity!=null)
                    {
                        sides.Add(new SideModel
                        {
                            Description=sideentity.Description,
                            Id=sideentity.ID,
                            InStock=sideentity.InStock,
                            Price=sideentity.Price,
                            Name=sideentity.Name,
                        });
                    }
                }
                //get the customer associated with the order
                var customerquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(x => x.Id == id));
                var customereneity = await customerquery(databaseContext, paged[i].CustomerId);
                orders.Add(new OrderModel
                {
                    Customer=customereneity.Map(),
                    CustomerId=customereneity.Id,
                    Pizzas=pizzas,
                    Sides=sides,
                    Status=paged.ElementAt(i).Status,
                    DateCreated=paged.ElementAt(i).DateCreated,
                    Id=paged.ElementAt(i).Id,
                    PizzaIds=paged.ElementAt(i).PizzaIds,
                    SideIds=paged.ElementAt(i).SideIds,
                });
            }

            return ListResult<OrderModel>.Success(orders, count);
        }

    }
}