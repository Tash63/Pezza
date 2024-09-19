using Common.Enums;

namespace Common.Models.Order
{

    public class SearchOrderModel
    {

        public string? OrderBy { get; set; }

        public PagingArgs? PagingArgs { get; set; } = PagingArgs.NoPaging;

        public int Id {  get; set; }

        public int? CustomerId { get; set; }

        public DateTime? DateCreated { get; set; }

        public OrderStatus? Status { get; set; }

    }

}