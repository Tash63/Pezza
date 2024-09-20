using Common.Models.Topping;
using System.Runtime.CompilerServices;

namespace Common.Mappers
{
    public static class ToppingMapper
    {

        public static ToppingModel Map (this Topping entity)
        {
            return new ToppingModel
            {
                Id = entity.Id,
                Name = entity.Name,
                PizzaId = entity.PizzaId,
                Price = entity.Price,
                Additional = entity.Additional,
                InStock = entity.InStock,
            };
        }

        public static Topping Map(this ToppingModel model)
        {
            return new Topping
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                PizzaId = model.PizzaId,
                Additional = model.Additional,
                InStock = model.InStock,
            };
        }

        public static IEnumerable<ToppingModel> Map(this IEnumerable<Topping> entitites)
        {
            return entitites.Select(x => x.Map());
        }

        public static List<ToppingModel> Map(this List<Topping> entitites)
        {
            return entitites.Select(x => x.Map()).ToList();
        }

        public static IEnumerable<Topping> Map(this IEnumerable<ToppingModel> models)
        {
            return models.Select(x => x.Map());
        }

        public static List<Topping> Map(this List<ToppingModel> models)
        {
            return models.Select(x => x.Map()).ToList();
        }

        public static List<List<ToppingModel>> Map(this List<List<Topping>> entitys)
        {
            List<List<ToppingModel>> result=new List<List<ToppingModel>>();
            for(int i=0;i<entitys.Count;i++)
            {
                result.Add(entitys[i].Map());
            }
            return result;
        }
    }
}
