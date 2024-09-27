using MediatR;

namespace Core.Customer.Commands;

public class DeleteApplicationUserCommand : IRequest<Result>
{
    public string? UserEmail { get; set; }
}

public class DeleteApplicationUserCommandHandler(DatabaseContext databaseContext) : IRequestHandler<DeleteApplicationUserCommand, Result>
{
    public async Task<Result> Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.UserEmail))
        {
            return Result.Failure("Error");
        }

        // find the entity in the db if exisits remove else return an error
        var ApplicationUserQuery = EF.CompileAsyncQuery((DatabaseContext db,string email)=>db.Users.FirstOrDefault(x=>x.Email==email));
        var ApplicationUserEntity = await ApplicationUserQuery(databaseContext,request.UserEmail);
        if (ApplicationUserEntity == null)
        {
            return Result.Failure("Not Found");
        }

        // Remove the eneity
        // TODO: remove the orders related to user? what will happen if we want to keep the data to make querys for stats
        databaseContext.Users.Remove(ApplicationUserEntity);
        var result= await databaseContext.SaveChangesAsync();

        return result > 0 ? Result.Success() : Result.Failure("Error");
    }
}