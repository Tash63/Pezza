using Common.Models.Side;
using LazyCache;
using MediatR;
namespace Core.Side.Command;
public class DeleteSideCommand : IRequest<Result>
{
    public int? Id { get; set; }
}

public class DeleteSideCommandHandler(DatabaseContext databaseContext, IAppCache cache) : IRequestHandler<DeleteSideCommand, Result>
{
    public async Task<Result> Handle(DeleteSideCommand request,CancellationToken cancellationToken)
    {
        if (request.Id == null)
        {
            return Result.Failure("Error");
        }
        var query=EF.CompileAsyncQuery((DatabaseContext db,int id)=>db.Sides.FirstOrDefault(p => p.ID == id));
        var findEntity = await query(databaseContext, request.Id.Value);
        if(findEntity==null)
        {
            return Result.Failure("Not found");
        }
        databaseContext.Sides.Remove(findEntity);
        var result=await databaseContext.SaveChangesAsync();
        return result>0 ?Result.Success():Result.Failure("Error");
    }
}
