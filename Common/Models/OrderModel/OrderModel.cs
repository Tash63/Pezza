using Common.Models.Customer;

namespace Common.Models.Order;

public class OrderModel
{
    public required CustomerModel Customer { get; set; }
    
    // One customer can have many Pizzas
    public required List<PizzaModel> Pizzas { get; set; }
}