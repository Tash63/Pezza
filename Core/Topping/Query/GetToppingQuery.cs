using Common.Models.Topping;

namespace Core.Topping.Query
{
    public class GetToppingQuery : IRequest<Result<ToppingModel>>
    {
        public int? Id { get; set; }
    }

    public class GetToppingQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetToppingQuery, Result<ToppingModel>>
    {
        public async Task<Result<ToppingModel>> Handle(GetToppingQuery request,CancellationToken cancellationToken)
        {
            if(request.Id==null)
            {
                return Result<ToppingModel>.Failure("Error");
            }

            //find if it exists
            var query = EF.CompileAsyncQuery((DatabaseContext db,int id)=>db.Toppings.FirstOrDefault(x=>x.Id==id));
            var result = await query(databaseContext, request.Id.Value);
            if(result==null)
            {
                return Result<ToppingModel>.Failure("Not Found");
            }
            return Result<ToppingModel>.Success(result.Map());
        }
    }
}

