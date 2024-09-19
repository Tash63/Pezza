using Common.Models.Topping;
using DataAcess.Filters;

namespace Core.Topping.Query
{
    public class GetToppingsQuery:IRequest<ListResult<ToppingModel>>
    {
        public SearchToppingModel? data {  get; set; }
    }

    public class GetToppingsQueryHandler(DatabaseContext databaseContext):IRequestHandler<GetToppingsQuery,ListResult<ToppingModel>>
    {
        public async Task<ListResult<ToppingModel>> Handle(GetToppingsQuery request,CancellationToken cancellationToken)
        {
            if(request.data==null)
            {
                return ListResult<ToppingModel>.Failure("Error");
            }

            if(request.data.OrderBy==null)
            {
                request.data.OrderBy="Price desc";
            }

            var entities = databaseContext.Toppings.
                Select(x => x)
                .AsNoTracking()
                .FiltterByPizza(request.data.PizzaID)
                .FilterByName(request.data.Name)
                .FilterByPrice(request.data.Price)
                .FilterByAdditional(request.data.Additional)
                .FilterByStock(request.data.InStock)
                .OrderBy(request.data.OrderBy);
            var count=entities.Count();
            var paged=await entities.ApplyPaging(request.data.PagingArgs).ToListAsync(cancellationToken);

            return ListResult<ToppingModel>.Success(paged.Map(), count);
        }
    }
}
