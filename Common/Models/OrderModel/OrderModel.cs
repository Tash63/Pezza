using Common.Enums;
using Common.Models.Customer;
using Common.Models.Side;
using Common.Models.Topping;
namespace Common.Models.Order;

public class OrderModel
{
    public int Id { get; set; }

    public required int CustomerId { get; set; }

    public CustomerModel Customer { get; set; }

    public List<PizzaModel> Pizzas { get; set; }

    public  List<SideModel> Sides { get; set; }

    public List<List<ToppingModel>> Toppings { get; set; }

    public DateTime? DateCreated { get; set; }

    public required OrderStatus Status { get; set; }
}