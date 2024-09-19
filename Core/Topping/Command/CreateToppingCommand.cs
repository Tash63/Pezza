using Common.Entities;
using Common.Models.Topping;
using System.Reflection.Metadata;

namespace Core.Topping.Command
{
    public class CreateToppingCommand: IRequest<Result<ToppingModel>>
    {
        public CreateToppingModel? data { get; set; }
    }

    public class CreateToppingCommandHandler(DatabaseContext databaseContext):IRequestHandler<CreateToppingCommand,Result<ToppingModel>>
    {
        public async Task<Result<ToppingModel>> Handle(CreateToppingCommand request,CancellationToken cancellationToken)
        {
            if(request.data==null)
            {
                return Result<ToppingModel>.Failure("Error");
            }

            // TODO: Add validation to check if the pizza id passed is asscoiated with a pizza

            var entity = new Common.Entities.Topping
            {
                Id = 0,
                PizzaId =request.data.PizzaID ,
                Additional=request.data.Additional ,
                InStock=request.data.InStcok,
                Name=request.data.Name ,
                Price=request.data.Price ,
            };

            // Save this entity into the db

            databaseContext.Toppings.Add(entity);

            // After adding an entity into a db table we need to save the chnages this goes for all operaitons on a db

            var result=await databaseContext.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result<ToppingModel>.Success(entity.Map()) : Result<ToppingModel>.Failure("Error");
        }
    }
}
