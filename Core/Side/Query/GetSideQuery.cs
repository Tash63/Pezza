using Common.Models.Side;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Side.Query;

public class GetSideQuery : IRequest<Result<SideModel>>
{
    public int? ID { get; set; }
}

public class GetSideQueryHandler(DatabaseContext databaseContext): IRequestHandler<GetSideQuery,Result<SideModel>>
{
    public async Task<Result<SideModel>> Handle(GetSideQuery request,CancellationToken cancellationToken)
    {
        if(request.ID==null)
        {
            return Result<SideModel>.Failure("Not Found");
        }
        var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(c => c.ID == id));
        var entity = await query(databaseContext, request.ID.Value);
        if (entity == null)
        {
            return Result<SideModel>.Failure("Not Found");
        }

        return Result<SideModel>.Success(entity.Map());
    }
}