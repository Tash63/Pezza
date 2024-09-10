using MediatR;

namespace Core.Customer.Commands;

public class DeleteCustomerCommand : IRequest<Result>
{
    public int? Id { get; set; }
}

public class DeleteCustomerCommandHandler(DatabaseContext databaseContext) : IRequestHandler<DeleteCustomerCommand, Result>
{
    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == null)
        {
            return Result.Failure("Error");
        }

        var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
        var findEntity = await query(databaseContext, request.Id.Value);
        if (findEntity == null)
        {
            return Result.Failure("Not found");
        }

        databaseContext.Customers.Remove(findEntity);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);

        return result > 0 ? Result.Success() : Result.Failure("Error");
    }
}