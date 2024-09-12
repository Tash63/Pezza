namespace Common.Mappers;

using Common.Entities;
using Common.Models.Customer;
using Common.Models.Order;
using System.Linq;
using System.Runtime.CompilerServices;

public static class OrderMapper
{

    private static List<PizzaModel> GetPizzaModels(ICollection<Pizza> Pizzas)
    {
        List<PizzaModel> pizzamodel = new List<PizzaModel>();
        for (int i = 0; i < Pizzas.Count(); i++)
        {
            pizzamodel.Add(Pizzas.ElementAt(i).Map());
        }
        return pizzamodel;
    }
    public static OrderModel Map(this Order entity)
        => new()
        { Id = entity.Id,
          Completed=entity.Completed,
        CustomerId=entity.CustomerId,
        Customer=entity.Customer.Map(),
        PizzaIds=entity.PizzaIds,
        Pizzas=entity.Pizzas.ToList().Map(),
        DateCreated=entity.DateCreated,
        };
    public static Order Map(this OrderModel model) =>
        new()
        {
            Id = model.Id,
            Completed = model.Completed,
            CustomerId = model.CustomerId,
            Customer = model.Customer.Map(),
            PizzaIds = model.PizzaIds,
            Pizzas = model.Pizzas.Map(),
            DateCreated = model.DateCreated,
        };
    public static Order Map(this CreateOrderModel model)
    => new()
    {
        Completed = false,
        CustomerId = model.CustomerId,
        PizzaIds = model.PizzaIds,
        DateCreated = DateTime.UtcNow
    };
    public static IEnumerable<OrderModel> Map(this List<Order> entities)
    => entities.Select(x => x.Map());

    public static IEnumerable<Order> Map(this List<OrderModel> models)
        => models.Select(x => x.Map());
}