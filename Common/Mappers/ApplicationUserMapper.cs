using Common.Models.ApplicationUser;
using Common.Models.Customer;
using System.Runtime.CompilerServices;

namespace Common.Mappers
{
    public static class ApplicationUserMapper
    {
        public static ApplicationUserModel Map(this ApplicationUser entity )
        {
            return new ApplicationUserModel
            {
                Adress = entity.Adress,
                DateCreated = entity.DateCreated.Value,
                Email=entity.Email,
                PhoneNumber=entity.PhoneNumber,
                FullName=entity.FullName,
            };
        }

        public static ApplicationUser Map(this ApplicationUserModel model)
        {
            return new ApplicationUser
            {
                Email = model.Email,
                DateCreated=model.DateCreated,
                Adress = model.Adress,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
            };
        }

        // TODO: Add the same mappers for the list of these
        public static IEnumerable<ApplicationUserModel> Map(this List<ApplicationUser> entities)
            => entities.Select(x => x.Map());

        public static IEnumerable<ApplicationUser> Map(this List<ApplicationUserModel> models)
            => models.Select(x => x.Map());
    }
}
