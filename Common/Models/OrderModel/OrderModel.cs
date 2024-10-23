using Common.Enums;
using Common.Mappers;
using Common.Models.ApplicationUser;
using Common.Models.Side;
using Common.Models.Topping;
namespace Common.Models.Order;

public class OrderModel
{
    public int Id { get; set; }

    public required string UserEmail { get; set; }

    public ApplicationUserModel User { get; set; }

    public List<PizzaModel> Pizzas { get; set; }

    public List<int> PizzaQuantity { get; set; }

    public  List<SideModel> Sides { get; set; }

    public List<int> SideQuantity { get; set; }

    public List<List<ToppingModel>> Toppings { get; set; }

    public DateTime? DateCreated { get; set; }

    public required OrderStatus Status { get; set; }
}