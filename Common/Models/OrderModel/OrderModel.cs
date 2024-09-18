using Common.Models.Customer;
using Common.Models.Side;
namespace Common.Models.Order;

public class OrderModel
{
    public int Id { get; set; }

    public required int CustomerId { get; set; }

    public required CustomerModel Customer { get; set; }

    public List<int> PizzaIds { get; set; } // List of Pizza IDs

    public List<int> SideIds { get; set; }

    public required List<PizzaModel> Pizzas { get; set; }

    public required List<SideModel> Sides { get; set; }

    public DateTime? DateCreated { get; set; }

    public required bool Completed { get; set; }
}