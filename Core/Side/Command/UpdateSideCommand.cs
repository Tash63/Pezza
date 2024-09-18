using Common.Models.Side;
using MediatR;

namespace Core.Side.Command;

public class UpdateSideCommand : IRequest<Result<SideModel>>
{
    public int? Id { get; set; }

    public UpdateSideModel? Data { get; set; }

}

public class UpdateSideCommandHandler(DatabaseContext databaseContext): IRequestHandler<UpdateSideCommand,Result<SideModel>>
{
    public async Task<Result<SideModel>> Handle(UpdateSideCommand request,CancellationToken cancellationToken)
    {
       if (request.Data == null || request.Id==null)
        {
            return Result<SideModel>.Failure("Error");
        }
       var model=request.Data;
        var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(c => c.ID == id));
        var findEntity = await query(databaseContext, request.Id.Value);
        if (findEntity == null)
        { 
            return Result<SideModel>.Failure("Not found");
        }
        findEntity.Description = string.IsNullOrEmpty(model.Description) ? findEntity.Description : model.Description;
        findEntity.Name = string.IsNullOrEmpty(model.Name)?findEntity.Name: model.Name;
        findEntity.Price=model.Price.HasValue?model.Price.Value:findEntity.Price;
        findEntity.InStock=model.InStock.HasValue?model.InStock.Value:findEntity.InStock;
        var outcome= databaseContext.Sides.Update(findEntity);
        var result= await databaseContext.SaveChangesAsync();

        return result > 0 ? Result<SideModel>.Success(findEntity.Map()) : Result<SideModel>.Failure("Error");
        
    }
}
