using Common.Models.Cart;

namespace Core.Customer.Queries;

public class GetCartQuery : IRequest<ListResult<AddToCartModel>>
{
    public int? CustomerID { get; set; }
}

public class GetCartQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetCartQuery, ListResult<AddToCartModel>>
{
    public async Task<ListResult<AddToCartModel>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        if(!request.CustomerID.HasValue)
        {
            return ListResult<AddToCartModel>.Failure("Error");
        }
        var entities = databaseContext.Carts
            .Select(x => x)
            .AsNoTracking()
            .Where(x=>x.CustomerId==request.CustomerID.Value).ToList();
        List<AddToCartModel> orders=new List<AddToCartModel>();
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
            orders.Add(new AddToCartModel
            {
                CustomerId = entities[i].CustomerId,
                PizzaID = entities[i].PizzaID,
                SideID = entities[i].SideID,
                ToppingIds = toppingIds
            });
        }
        return ListResult<AddToCartModel>.Success(orders, orders.Count());
    }
}