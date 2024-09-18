using Common.Models.Side;
using DataAcess.Filters;

namespace Core.Side.Query;

public class GetSideQuerys:IRequest<ListResult<SideModel>>
{
    public SearchSideModel Data { get; set; }
}

public class GetSideQuerysHandler(DatabaseContext databaseContext):IRequestHandler<GetSideQuerys,ListResult<SideModel>>
{
    public async Task<ListResult<SideModel>> Handle(GetSideQuerys request,CancellationToken cancellationToken)
    {
        var entity=request.Data;
        if(string.IsNullOrEmpty(entity.OrderBy))
        {
            entity.OrderBy = "Price desc";
        }
        var entitiees = databaseContext.Sides
            .Select(x => x)
            .AsNoTracking()
            .FilterByInStock(entity.InStock)
            .FilterByName(entity.Name)
            .OrderBy(entity.OrderBy);
        var count=entitiees.Count();
        var paged = await entitiees.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

        return ListResult<SideModel>.Success(paged.Map(), count);
    }
}