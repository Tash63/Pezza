using Common.Models.Topping;
using DataAcess.Filters;

namespace Core.Topping.Query
{
    public class GetToppingsQuery:IRequest<ListResult<ToppingModel>>
    {
        public SearchToppingModel? Data {  get; set; }
    }

    public class GetToppingsQueryHandler(DatabaseContext databaseContext):IRequestHandler<GetToppingsQuery,ListResult<ToppingModel>>
    {
        public async Task<ListResult<ToppingModel>> Handle(GetToppingsQuery request,CancellationToken cancellationToken)
        {
            if(request.Data==null)
            {
                return ListResult<ToppingModel>.Failure("Error");
            }

            if(string.IsNullOrEmpty(request.Data.OrderBy))
            {
                request.Data.OrderBy="Price desc";
            }

            var entities = databaseContext.Toppings.
                Select(x => x)
                .AsNoTracking()
                .FiltterByPizza(request.Data.PizzaID)
                .FilterByName(request.Data.Name)
                .FilterByPrice(request.Data.Price)
                .FilterByAdditional(request.Data.Additional)
                .FilterByStock(request.Data.InStock)
                .OrderBy(request.Data.OrderBy);
            var count=entities.Count();
            var paged=await entities.ApplyPaging(request.Data.PagingArgs).ToListAsync(cancellationToken);

            return ListResult<ToppingModel>.Success(paged.Map(), count);
        }
    }
}
