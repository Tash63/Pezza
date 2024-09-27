namespace Core.Pizza.Queries;

using Common.Entities;

public class GetNotifiesQuery : IRequest<ListResult<Notify>>
{
}

public class GetNotifiesQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetNotifiesQuery, ListResult<Notify>>
{
    public async Task<ListResult<Notify>> Handle(GetNotifiesQuery request, CancellationToken cancellationToken)
    {
        var result = await databaseContext.SaveChangesAsync(cancellationToken);
        var entities = databaseContext.Notifies
            .Select(x => x)
            .Where(x => x.Sent == false)
            .AsNoTracking();

        var count = entities.Count();
        var data = await entities.ToListAsync(cancellationToken);

        return ListResult<Notify>.Success(data, count);
    }
}