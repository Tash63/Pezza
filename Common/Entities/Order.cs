using Common.Enums;
using Common.Mappers;

namespace Common.Entities;

public class Order
{

    public int Id { get; set; }

    public required int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }

    public DateTime? DateCreated { get; set; }

    public required OrderStatus Status { get; set; }

    public List<int> SideIds { get; set; }

    public ICollection<Side> Sides { get; set; }    


}