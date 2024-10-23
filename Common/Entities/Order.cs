using Common.Enums;
using Common.Mappers;

namespace Common.Entities;

public class Order
{

    public int Id { get; set; }
    public required string UserEmail { get; set; }
    public virtual ApplicationUser User { get; set; }
    public DateTime? DateCreated { get; set; }
    public required OrderStatus Status { get; set; }

}