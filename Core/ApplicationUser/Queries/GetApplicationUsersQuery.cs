using Common.Models.ApplicationUser;

namespace Core.Customer.Queries;

public class GetApplicationUsersQuery : IRequest<ListResult<ApplicationUserModel>>
{
    public SearchApplicationUserModel Data { get; set; }
}

public class GetApplicationUsersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetApplicationUsersQuery, ListResult<ApplicationUserModel>>
{
    public async Task<ListResult<ApplicationUserModel>> Handle(GetApplicationUsersQuery request, CancellationToken cancellationToken)
    {
        var entity = request.Data;
        if (string.IsNullOrEmpty(entity.OrderBy))
        {
            entity.OrderBy = "DateCreated desc";
        }
        var entities = databaseContext.Users
            .Select(x => x)
            .AsNoTracking()
            .FilterByName(entity.FullName)
            .FilterByAddress(entity.Address)
            .FilterByPhone(entity.Cellphone)
            .FilterByEmail(entity.Email)
            .OrderBy(entity.OrderBy);

        var count = entities.Count();
        var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

        return ListResult<ApplicationUserModel>.Success(paged.Map(), count);
    }
}