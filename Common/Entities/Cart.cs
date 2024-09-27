namespace Common.Entities
{
    public sealed class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? PizzaID { get; set; }

        public int? SideID {  get; set; }
        public string UserEmail { get; set; }
    }
}
