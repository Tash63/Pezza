namespace Common.Models.Cart
{
    public class AddToCartModel
    {
        public required int CustomerId { get; set; }

        public int? PizzaID { get; set; }

        public int? SideID { get; set; }

        public List<int> ToppingIds { get; set; }
    }
}
