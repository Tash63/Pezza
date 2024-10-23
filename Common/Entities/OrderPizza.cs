namespace Common.Entities
{
    public class OrderPizza
    {
        public int Id { get; set; }

        public int? PizzaId { get; set; }

        public int? SideID { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }

    }
}
