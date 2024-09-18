using Common.Models.Side;
using MediatR;

namespace Core.Side.Command;

public class CreateSideCommand : IRequest<Result<SideModel>>
{
    public CreateSideModel? Data { get; set; }
}

public class CreateSideCommandHandler(DatabaseContext databaseContext): IRequestHandler<CreateSideCommand, Result<SideModel>>
{
    public async Task<Result<SideModel>> Handle(CreateSideCommand request,CancellationToken cancellationToken)
    {
        if(request.Data==null)
        {
            return Result<SideModel>.Failure("Error");
        }
        var enitty = new Common.Entities.Side
        {
            ID=0,
            Description=request.Data.Description,
            InStock=request.Data.InStock,
            Name=request.Data.Name,
            Price=request.Data.Price,
        };
        databaseContext.Sides.Add(enitty);
        var result= await databaseContext.SaveChangesAsync(cancellationToken);

        return result>0? Result<SideModel>.Success(enitty.Map()):Result<SideModel>.Failure("Error"); 
    }

}