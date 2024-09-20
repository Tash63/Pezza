namespace Common.Entities
{
    public class OrderPizzaTopping
    {
        public int Id { get; set; }

        public int OrderPizzaId { get; set; }

        public int ToppingId { get; set; }
    }
}
