namespace Common.Entities
{
   public class Side
    {
        public required int ID { get; set; }
        public string? Name { get; set; }    
        public string? Description { get; set; } 
        public double Price { get; set; }
        public bool InStock {  get; set; }
    }
}
