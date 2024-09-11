using MediatR;

namespace Core.Pizza.Queries;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
    public SearchPizzaModel Data { get; set; }
}

public class GetPizzasQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
{
    /*so the models are used to expose certain things we need from the body of the request inorder to forfuil the request like here we added paging parameters to the search models and thats whats 
     exposed and the filter we can return the query when entred if we check if that filed was not filled in the model
    and the where clause appends a where when we call filter*/
    public async Task<ListResult<PizzaModel>> Handle(GetPizzasQuery request, CancellationToken cancellationToken)
    {

        var entity = request.Data;
        //add a switch statement to this and the customer one incase something defualts and we get 500
        if(string.IsNullOrEmpty(entity.OrderBy))
        {
            entity.OrderBy = "DateCreated desc";
        }


        var entities = databaseContext.Pizzas.Select(x => x).AsNoTracking().FilterByDate(entity.DateCreated).FilterByName(entity.Name).FilterByPrice((float)entity.Price).OrderBy(entity.OrderBy);

        var count = entities.Count();
        var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

        return ListResult<PizzaModel>.Success(paged.Map(), count);
    }
}