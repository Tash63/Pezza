using Common.Models.Cart;

namespace Core.Customer.Queries;

public class GetCartQuery : IRequest<ListResult<SearchCartModel>>
{
    public string? CustomerEmail { get; set; }
}

public class GetCartQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetCartQuery, ListResult<SearchCartModel>>
{
    public async Task<ListResult<SearchCartModel>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.CustomerEmail))
        {
            return ListResult<SearchCartModel>.Failure("Error");
        }
        var entities = databaseContext.Carts
            .Select(x => x)
            .AsNoTracking()
            .Where(x=>x.UserEmail==request.CustomerEmail).ToList();
        List<SearchCartModel> orders=new List<SearchCartModel>();
        for(int i=0;i<entities.Count();i++)
        {
            // if the current order is a pizza then find the toppings
            List<int> toppingIds=new List<int>();
            if (entities[i].PizzaID!=null)
            {
                var toppingentities = databaseContext.CartToppings
                    .Select(x => x)
                    .AsNoTracking()
                    .Where(x => x.CartID == entities[i].Id).ToList();
                for(int j=0;j<toppingentities.Count();j++)
                {
                    toppingIds.Add(toppingentities[j].ToppingId);
                }
            }
            orders.Add(new SearchCartModel
            {
                UserEmail=request.CustomerEmail,
                PizzaID = entities[i].PizzaID,
                SideID = entities[i].SideID,
                ToppingIds = toppingIds,
                Id=entities[i].Id
            });
        }
        return ListResult<SearchCartModel>.Success(orders, orders.Count());
    }
}