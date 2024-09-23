namespace Core.Cart.Command
{
    public class RemoveFromCart: IRequest<Result>
    {
        public int? Id {get; set; }
    }

    public class RemoveFromCartHandler(DatabaseContext databaseContext):IRequestHandler<RemoveFromCart,Result>
    {
        public async Task<Result> Handle(RemoveFromCart request, CancellationToken cancellationToken)
        {
            if(request.Id==null)
            {
                return Result.Failure("Error");
            }

            var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Carts.FirstOrDefault(c => c.Id == id));
            var findEntity = await query(databaseContext, request.Id.Value);
            if (findEntity == null)
            {
                return Result.Failure("Not found");
            }
            // TODO: perform validation on the topping id before adding toppijng
            var toppingsToDelete = databaseContext.CartToppings
                .Where(t => t.CartID.Equals(findEntity.Id))
                .ToList(); 

            databaseContext.CartToppings.RemoveRange(toppingsToDelete);
            databaseContext.SaveChanges();

            databaseContext.Carts.Remove(findEntity);
            var result= await databaseContext.SaveChangesAsync();
            return result>0?Result.Success():Result.Failure("Error");
        }
    }
}
 