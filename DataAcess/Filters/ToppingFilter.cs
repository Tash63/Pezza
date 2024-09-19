using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;

namespace DataAcess.Filters
{
    public static class ToppingFilter
    {

        // Have to use the entity type for the generic becuase this is an operation on a database table wich is
        // type of the eneity

        public static IQueryable<Topping> FiltterByPizza(this IQueryable<Topping> query,int? pizzaId)
        {
            if (!pizzaId.HasValue)
            {
                return query;
            }
            return query.Where(x => x.PizzaId == pizzaId.Value);
        }
        public static IQueryable<Topping> FilterByName(this IQueryable<Topping> query,string? name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return query;
            }
            return query.Where(x => x.Name == name);
        }
        // TODO : Improve the price filtering by allowing user to enter a range to filtter through
        public static IQueryable<Topping> FilterByPrice(this IQueryable<Topping> query, double? price)
        {
            if(!price.HasValue)
            {
                return query;
            }
            return query.Where(x => x.Price==price.Value);
        }

        public static IQueryable<Topping> FilterByAdditional(this IQueryable<Topping> qurey,bool? additional)
        {
            if(!additional.HasValue)
            {
                return qurey;
            }
            return qurey.Where(x => x.Additional == additional.Value);
        }

        public static IQueryable<Topping> FilterByStock(this IQueryable<Topping> query,bool? instock)
        {
            if(!instock.HasValue)
            {
                return query;
            }
            return query.Where(x => x.InStock == instock.Value);
        }

    }
}
