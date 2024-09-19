using Common.Enums;

namespace Common.Models.Pizza
{
    public class SearchPizzaModel
    {
        public string? OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; } = PagingArgs.NoPaging;
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public DateTime? DateCreated { get; set; }

        public PizzaCategory? Category { get; set; }

        public bool? InStock { get; set; }
    }
}
