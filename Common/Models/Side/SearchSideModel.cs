namespace Common.Models.Side
{
    public class SearchSideModel
    {
        public string? OrderBy { get; set; }
        public PagingArgs? PagingArgs { get; set; }=PagingArgs.NoPaging;

        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        // TODO: Add fillter for the sides and the filtter for the in stock select all in Pizzas

        public bool? InStock { get; set; }
    }
}
