namespace Common.Models.Topping
{
    public class SearchToppingModel
    {
        public int Id { get; set; }

        public int? PizzaID { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public bool? InStock { get; set; }

        public bool? Additional { get; set; }

        public string ? OrderBy { get; set; }

        public PagingArgs? PagingArgs { get; set; }=PagingArgs.NoPaging;

    }
}
