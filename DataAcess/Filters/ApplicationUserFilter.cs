namespace Common.Filters;

public static class ApplicationUserFilter
{
    public static IQueryable<ApplicationUser> FilterByName(this IQueryable<ApplicationUser> query, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return query;
        }

        return query.Where(x => x.FullName.Contains(name));
    }

    public static IQueryable<ApplicationUser> FilterByAddress(this IQueryable<ApplicationUser> query, string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return query;
        }

        return query.Where(x => x.Adress.Contains(address));
    }

    public static IQueryable<ApplicationUser> FilterByPhone(this IQueryable<ApplicationUser> query, string cellphone)
    {
        if (string.IsNullOrWhiteSpace(cellphone))
        {
            return query;
        }

        return query.Where(x => x.PhoneNumber.Contains(cellphone));
    }

    public static IQueryable<ApplicationUser> FilterByEmail(this IQueryable<ApplicationUser> query, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return query;
        }

        return query.Where(x => x.Email.Contains(email));
    }

    public static IQueryable<ApplicationUser> FilterByDateCreated(this IQueryable<ApplicationUser> query, DateOnly? dateCreated)
    {
        if (!dateCreated.HasValue)
        {
            return query;
        }

        return query.Where(x => x.DateCreated == dateCreated.Value);
    }
}