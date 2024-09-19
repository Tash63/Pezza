namespace Common.Models.Topping
{
    public class CreateToppingModel
    {

        public string Name { get; set; }

        public double Price { get; set; }

        public required int  PizzaID { get; set; }    
        
        public bool Additional { get; set; }

        public bool InStcok { get; set; }

    }
}
