namespace Core.Order.Commands;

using Common.Entities;
using Common.Models.Order;
using Core.Order.Events;
using DataAccess;
using MediatR;

public class OrderCommand : IRequest<Result>
{
    public string? CustomerEmail { get; set; }
}

public class OrderCommandHandler(IMediator mediator, DatabaseContext databaseContext) : IRequestHandler<OrderCommand, Result>
{

    public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.CustomerEmail))
        {
            return Result.Failure("Error");
        }

        // the order infomation will be stored in the cart so i will build the request.data model together
        // with insertion into the db

        // get all cart items for the customer
        var cartitems = databaseContext.Carts.Select(x => x).
            AsNoTracking()
            .Where(x => x.UserEmail == request.CustomerEmail).ToList();
        List<int> PizzaIDs = new List<int>();
        List<int> PizzaQuantity = new List<int>();
        List<int> SideIDs = new List<int>();
        List<int> SideQuantity = new List<int>();
        List<List<int>> ToppingIDs = new List<List<int>>();

        if (cartitems.Count == 0)
        {
            return Result.Failure("No Items in Cart");
        }

        for (int i = 0; i < cartitems.Count(); i++)
        {
            if (cartitems[i].PizzaID == null)
            {
                //get the ids from the cart for the side and its quenity
                SideQuantity.Add(cartitems[i].Quantity);
                SideIDs.Add(cartitems[i].SideID.Value);
            }
            else
            {
                //get the ids form the cart for the pizza and its quantity
                PizzaQuantity.Add(cartitems[i].Quantity);
                PizzaIDs.Add(cartitems[i].PizzaID.Value);
                // if its a pizza in this cart item we need to get the toppings for it from the carttoppings
                var toppingitems = databaseContext.CartToppings.Select(x => x)
                    .AsNoTracking()
                    .Where(x => x.CartID == cartitems[i].Id).ToList();
                List<int> ToppingId = new List<int>();
                for (int j = 0; j < toppingitems.Count(); j++)
                {
                    ToppingId.Add(toppingitems[j].ToppingId);
                }
                ToppingIDs.Add(ToppingId);
            }
        }
        // TODO: remove the cart items from the cart table

        var order = new Order
        {
            Status = Common.Enums.OrderStatus.Placed,
            UserEmail = request.CustomerEmail,
            DateCreated = DateTime.UtcNow,
        };
        databaseContext.Orders.Add(order);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            for (int i = 0; i < cartitems.Count; i++)
            {
                if (cartitems[i].PizzaID != null)
                {
                    var toppingsToDelete = databaseContext.CartToppings
                        .Where(t => t.CartID.Equals(cartitems[i].Id))
                        .ToList();
                    databaseContext.CartToppings.RemoveRange(toppingsToDelete);
                    databaseContext.SaveChanges();
                }

                // remove this cart entity
                databaseContext.Carts.Remove(cartitems[i]);
                await databaseContext.SaveChangesAsync(cancellationToken);
            }
        }
        // Add Pizza's to that order
        int LastOrderId = order.Id;
        for (int i = 0; i < PizzaIDs.Count; i++)
        {
            var OrderPizzaEntity = new OrderPizza
            {
                OrderId = LastOrderId,
                PizzaId = PizzaIDs[i],
                SideID = null,
                Quantity = PizzaQuantity[i],
            };

            databaseContext.OrderPizzas.Add(OrderPizzaEntity);
            await databaseContext.SaveChangesAsync(cancellationToken);
            // Add the toppings for this pizza in the orderpizzatoppings
            for (int j = 0; j < ToppingIDs[i].Count; j++)
            {
                //check if the toppings exisit
                databaseContext.OrderPizzaToppings.Add(new OrderPizzaTopping
                {
                    OrderPizzaId = OrderPizzaEntity.Id,
                    ToppingId = ToppingIDs[i][j],
                });
                await databaseContext.SaveChangesAsync(cancellationToken);
            }
        }
        // add sideids to the same table as the pizza
        for (int i = 0; i < SideIDs.Count; i++)
        {
            var OrderPizzaEntity = new OrderPizza
            {
                OrderId = LastOrderId,
                Quantity = SideQuantity[i],
                SideID = SideIDs[i],
                PizzaId = null
            };
            databaseContext.OrderPizzas.Add(OrderPizzaEntity);
            await databaseContext.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }
}

// TODO: call a roll back if we cant sucefully insert the order into the db if forexample an incorrect entry has been made