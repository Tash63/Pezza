namespace Common.Entities
{
    public sealed class CartTopping
    {
        public int Id { get; set; }
        required public int CartID {  get; set; }
        required public int ToppingId {  get; set; }
    }
}
