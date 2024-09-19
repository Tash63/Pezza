using Common.Models.Topping;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection.Metadata;

namespace Core.Topping.Command
{
    public class UpdateToppingCommand: IRequest<Result<ToppingModel>>
    {
        public UpdateToppingModel? data;

        public int? Id;
    }

    public class UpdateToppingCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateToppingCommand, Result<ToppingModel>>
    {

        // Pass the request for update topping (so it can get the request data )as well as the cancellation token into the handle method
        public async Task<Result<ToppingModel>> Handle(UpdateToppingCommand request, CancellationToken cancellationToken)
        {
            if (request.data == null || request.Id.HasValue)
            {
                return Result<ToppingModel>.Failure("Error");
            }

            var model=request.data;

            // check if the id entred exists in the db

            // TODO : Add validation to check if the pizza id exists
            var query=EF.CompileAsyncQuery((DatabaseContext db,int id) => db.Toppings.FirstOrDefault(x=>x.Id == id));
            var result = await query(databaseContext, request.Id.Value);
            
            if(result==null)
            {
                return Result<ToppingModel>.Failure("Error");
            }

            // Update the contents of results if it exists within model

            result.Name = string.IsNullOrEmpty( model.Name)?result.Name : model.Name;
            result.Price = model.Price.HasValue ? model.Price.Value : result.Price;
            result.Additional=model.Additional.HasValue?model.Additional.Value:result.Additional;
            result.InStock = model.InStock.HasValue ? model.InStock.Value : result.InStock;

            databaseContext.Toppings.Update(result);
            var updateresult = await databaseContext.SaveChangesAsync();
            return updateresult > 0 ? Result<ToppingModel>.Success(result.Map()) : Result<ToppingModel>.Failure("Error");
        }

    }
}
