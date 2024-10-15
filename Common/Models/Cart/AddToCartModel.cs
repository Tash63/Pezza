namespace Common.Models.Cart
{
    public class AddToCartModel
    {
        public required string UserEmail { get; set; }

        public int? PizzaID { get; set; }

        public int? SideID { get; set; }

        public int Quantity { get; set; }

        public List<int> ToppingIds { get; set; }
    }
}
