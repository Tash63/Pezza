using Common.Entities;
using Common.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Core.Customer.Commands;

public class UpdateApplicationUserCommand : IRequest<Result<UpdateApplicationUserModel>>
{
    public string? UserEmailAdress { get; set; }

    public UpdateApplicationUserModel? Data { get; set; }
}

public class UpdateApplicationUserCommandHandler(DatabaseContext databaseContext,UserManager<ApplicationUser> userManager) : IRequestHandler<UpdateApplicationUserCommand, Result<UpdateApplicationUserModel>>
{
    public async Task<Result<UpdateApplicationUserModel>> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
    {
        if(request.Data==null || string.IsNullOrEmpty(request.UserEmailAdress))
        {
            return Result<UpdateApplicationUserModel>.Failure("Error");
        }

        // find if the user exisits
        var ApplicationUserQuery = EF.CompileAsyncQuery((DatabaseContext db,string id)=>db.Users.FirstOrDefault(x=>x.Email==id));
        var ApplicationUserResult = await ApplicationUserQuery(databaseContext, request.UserEmailAdress);
        if (ApplicationUserResult == null)
        {
            return Result<UpdateApplicationUserModel>.Failure("Not Found");
        }

        // update user informaiton
        ApplicationUserResult.PhoneNumber = string.IsNullOrEmpty(request.Data.PhoneNumber) ? ApplicationUserResult.PhoneNumber : request.Data.PhoneNumber;
        ApplicationUserResult.Adress=string.IsNullOrEmpty(request.Data.Adress)?ApplicationUserResult.FullName: request.Data.Adress;
        ApplicationUserResult.FullName=string.IsNullOrEmpty(request.Data.Fullname)?ApplicationUserResult.FullName : request.Data.Fullname;
        //set user password
        PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
        ApplicationUserResult.PasswordHash =string.IsNullOrEmpty(request.Data.Password)?ApplicationUserResult.PasswordHash: ph.HashPassword(ApplicationUserResult, request.Data.Password);

        // Update user entity in data base
        var outcome = databaseContext.Users.Update(ApplicationUserResult);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);
        //TODO :Modify the request to alllow updat of the user email
        if(result>0)
        {

            // modify request data with contents in dB
            request.Data.Adress = ApplicationUserResult.Adress;
            request.Data.Adress = ApplicationUserResult.Adress;
            request.Data.PhoneNumber = ApplicationUserResult.PhoneNumber;
        }
        return result > 0 ? Result<UpdateApplicationUserModel>.Success(request.Data) : Result<UpdateApplicationUserModel>.Failure("Error");
    }
}