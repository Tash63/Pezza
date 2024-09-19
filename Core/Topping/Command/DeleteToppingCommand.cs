
using Common.Models.Topping;

namespace Core.Topping.Command
{

    public class DeleteToppingCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }

    public class DeleteToppingCommandHandler(DatabaseContext databaseContext):IRequestHandler<DeleteToppingCommand,Result>
    {

        public async Task<Result> Handle(DeleteToppingCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
            {
                return Result.Failure("Error");
            }
            //check if this ID exisits in the DB

            var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Toppings.FirstOrDefault(x => x.Id == id));
            var toppingentity = await query(databaseContext, request.Id.Value);
            if (toppingentity == null)
            {
                return Result.Failure("Not Found");
            }

            // remove topping from db
            databaseContext.Toppings.Remove(toppingentity);
            var result=await databaseContext.SaveChangesAsync();

            return result > 0 ? Result.Success() : Result.Failure("Error");
        }

    }
}