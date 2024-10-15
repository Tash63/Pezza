namespace Common.Models.Cart
{
    public class SearchCartModel
    {
        public string UserEmail { get; set; }
        public int? PizzaID { get; set; }

        public int? SideID { get; set; }

        public int Id { get; set; }

        public int Quantity { get; set; }
        public List<int> ToppingIds { get; set; }
    }
}
