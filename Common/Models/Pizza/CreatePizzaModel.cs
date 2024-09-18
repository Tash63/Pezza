namespace Common.Models.Pizza
{
    public class CreatePizzaModel
    {

        public  string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public DateTime? DateCreated { get; set; }

        public PizzaCategory Category { get; set; }
        public bool InStock {  get; set; }
    }
}
