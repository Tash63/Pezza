namespace Common.Models.Cart
{
    public class SearchCartModel
    {
        public int CustomerId { get; set; }

        public int? PizzaID { get; set; }

        public int? SideID { get; set; }

        public int Id { get; set; }

        public List<int> ToppingIds { get; set; }
    }
}
