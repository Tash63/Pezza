using Common.Enums;

namespace Common.Models.Order
{

    public class SearchOrderModel
    {

        public string? OrderBy { get; set; }

        public PagingArgs? PagingArgs { get; set; } = PagingArgs.NoPaging;

        public int? Id {  get; set; }

        public string? UserEmail { get; set; }

        public DateOnly? DateCreated { get; set; }

        public OrderStatus? Status { get; set; }

    }

}