using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;

namespace Common.Filters
{
    public static class PizzaFilter
    {
        //the Iquerable is a generic which we can pass a type wich is Pizza so it acesses the contents of it
        public static IQueryable<Pizza> FilterByName(this IQueryable<Pizza> query,string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return query;
            }
            return query.Where(x => x.Name.Contains( name));
        }

        public static IQueryable<Pizza> FilterByPrice(this IQueryable<Pizza> query,float? price)
        {
            if(!price.HasValue)
            {
                return query;
            }
            //nullable types can get the actual value by using the nullable.Value
            return query.Where(x => x.Price == (decimal)price.Value);
        }

        public static IQueryable<Pizza> FilterByDate(this IQueryable<Pizza> query,DateTime? date)
        {
            if(!date.HasValue)
            {
                return query;
            }
            return query.Where(x => x.DateCreated == date.Value);
        }
    }
}