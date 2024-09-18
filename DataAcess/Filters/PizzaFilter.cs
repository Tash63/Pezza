namespace Common.Filters;

using Common.Models;
using Common.Models.Pizza;
using System.Diagnostics.CodeAnalysis;

public static class PizzaFilter
{
    public static IQueryable<Pizza> FilterByName(this IQueryable<Pizza> query, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return query;
        }

        return query.Where(x => x.Name.Contains(name));
    }

    public static IEnumerable<PizzaModel> FilterByName(this IEnumerable<PizzaModel> query, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return query;
        }

        return query.Where(x => x.Name.Contains(name));
    }

    public static IQueryable<Pizza> FilterByDescription(this IQueryable<Pizza> query, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return query;
        }

        return query.Where(x => x.Description.Contains(description));
    }

    public static IEnumerable<PizzaModel> FilterByDescription(this IEnumerable<PizzaModel> query, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return query;
        }

        return query.Where(x => x.Description.Contains(description));
    }

    public static IQueryable<Pizza> FilterByDateCreated(this IQueryable<Pizza> query, DateTime? dateCreated)
    {
        if (!dateCreated.HasValue)
        {
            return query;
        }

        return query.Where(x => x.DateCreated == dateCreated.Value);
    }

    public static IEnumerable<PizzaModel> FilterByDateCreated(this IEnumerable<PizzaModel> query, DateTime? dateCreated)
    {
        if (!dateCreated.HasValue)
        {
            return query;
        }

        return query.Where(x => x.DateCreated == dateCreated.Value);
    }

    public static IQueryable<Pizza> FilterByStock(this IQueryable<Pizza> query, bool? IsInStock)
    {
        if(!IsInStock.HasValue)
        {
            return query;
        }
        return query.Where(x => x.InStock == IsInStock.Value);
    }

    public static IEnumerable<PizzaModel> FilterByStock(this IEnumerable<PizzaModel> query, bool? IsInStock)
    {
        if (!IsInStock.HasValue)
        {
            return query;
        }
        return query.Where(o => o.InStock == IsInStock.Value);
    }

    public static IQueryable<Pizza>FilterByCatergory(this IQueryable<Pizza> query,PizzaCategory? cat)
    {
        if(!cat.HasValue)
        {
            return query;
        }
        return query.Where(x=>x.Category.Equals(cat));
    }

    public static IEnumerable<PizzaModel> FilterByCatergory(this IEnumerable<PizzaModel> query, PizzaCategory? cat)
    {
        if (!cat.HasValue)
        {
            return query;
        }
        return query.Where(x => x.Category.Equals(cat));
    }
}