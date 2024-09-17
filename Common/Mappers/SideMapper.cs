
using Common.Models.Side;
using System.Runtime.CompilerServices;

namespace Common.Mappers
{
    public static class SideMapper
    {
        //purpose of this is to convert between the enityt in the db to the model used

        //this is an extension mehtod since we are passing the enity itself with this this keyword
        public static SideModel Map( this Side enitty)
        {
            return new SideModel
            {
                Id=enitty.ID,
                Description=enitty.Description,
                InStock=enitty.InStock,
                Name = enitty.Name,
                Price = enitty.Price
            };
        }

        public static Side Map(this SideModel model)
        {
            return new Side
            {
                ID=model.Id,
                Description=model.Description,
                InStock=model.InStock,
                Name = model.Name,
                Price = model.Price,
            };
        }

        // to convert lists back and forth
        public static List<SideModel> Map(this List<Side> entitys)
        {
            return entitys.Select(entity => entity.Map()).ToList();
        }

        public static List<Side> Map( this List<SideModel> models)
        {
            return models.Select(model => model.Map()).ToList();
        }
    }
}
