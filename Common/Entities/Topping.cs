using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Common.Entities
{
    public class Topping
    {
        public required int Id { get; set; }

        public string Name { get; set; }

        public required int PizzaId { get; set; }

        public double Price { get; set; }


        // If true then the topping is not standard and therfore is an additonal topping
        // If false then the topping is standard and therefore removable
        public bool Additional {  get; set; }

        public bool InStock { get; set; }

    }
}
