using Common.Models.Cart;

namespace Core.Cart.Command
{
    public class AddToCart : IRequest<Result<AddToCartModel>>
    {
        public AddToCartModel? Data { get; set; }
    }

    public class AddToCartHandler(DatabaseContext databaseContext) : IRequestHandler<AddToCart, Result<AddToCartModel>>
    {
        public async Task<Result<AddToCartModel>> Handle(AddToCart request, CancellationToken cancellationToken)
        {
            if (request.Data == null)
            {
                return Result<AddToCartModel>.Failure("Error");
            }

            // create an entry into the cart table and enter the username and either side or pizza entity
            var entity = new Common.Entities.Cart
            {
                PizzaID = request.Data.PizzaID,
                SideID = request.Data.SideID,
                UserEmail = request.Data.UserEmail,
            };
            databaseContext.Carts.Add(entity);
            var result = await databaseContext.SaveChangesAsync(cancellationToken);

            // if the pizzaID is not null make additions to the toppings
            if (request.Data.PizzaID.HasValue && result > 0)
            {
                for (var i = 0; i < request.Data.ToppingIds.Count; i++)
                {
                    databaseContext.CartToppings.Add(new Common.Entities.CartTopping
                    {
                        CartID = entity.Id,
                        ToppingId = request.Data.ToppingIds[i],
                    });
                    databaseContext.SaveChanges();
                }
            }
            return result > 0 ? Result<AddToCartModel>.Success(request.Data) : Result<AddToCartModel>.Failure("Error");
        }
    }
}
