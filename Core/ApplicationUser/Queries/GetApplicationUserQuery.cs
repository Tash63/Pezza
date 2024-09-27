using Common.Models.ApplicationUser;
using MediatR;

namespace Core.Customer.Queries;

public class GetApplicationUserQuery : IRequest<Result<ApplicationUserModel>>
{
    public string? Email { get; set; }
}

public class GetApplicationUserHandler(DatabaseContext databaseContext) : IRequestHandler<GetApplicationUserQuery, Result<ApplicationUserModel>>
{
    public async Task<Result<ApplicationUserModel>> Handle(GetApplicationUserQuery request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.Email))
        {
            return Result<ApplicationUserModel>.Failure("Error"); 
        }
        var ApplicationUserQuery = EF.CompileAsyncQuery((DatabaseContext db,string email)=>db.Users.FirstOrDefault(x=>x.Email==email));
        var ApplicationUserResult = await ApplicationUserQuery(databaseContext, request.Email);
        if (ApplicationUserResult == null)
        {
            return Result<ApplicationUserModel>.Failure("Not Found");
        }

        return Result<ApplicationUserModel>.Success(ApplicationUserResult.Map());
    }
}