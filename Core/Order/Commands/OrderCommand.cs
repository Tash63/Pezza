namespace Core.Order.Commands;

using Common.Entities;
using Common.Models.Order;
using Core.Order.Events;
using DataAccess;
using MediatR;
using System.Security.Principal;

public class OrderCommand : IRequest<Result>
{
    public int? CustomerId { get; set; }
}

public class OrderCommandHandler(IMediator mediator,DatabaseContext databaseContext) : IRequestHandler<OrderCommand, Result>
{

    public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
    {
        if (!request.CustomerId.HasValue)
        {
            return Result.Failure("Error");
        }

        // the order infomation will be stored in the cart so i will build the request.data model together
        // with insertion into the db

        // get all cart items for the customer
        var cartitems=databaseContext.Carts.Select(x=>x).
            AsNoTracking()
            .Where(x => x.CustomerId == request.CustomerId.Value).ToList();
        List<int> PizzaIDs = new List<int>();
        List<int> SideIDs = new List<int>();
        List<List<int>> ToppingIDs=new List<List<int>>();

        if(cartitems.Count==0)
        {
            return Result.Failure("No Items in Cart");
        }

        for(int i=0;i<cartitems.Count();i++)
        {
            if (cartitems[i].PizzaID==null)
            {
                SideIDs.Add(cartitems[i].SideID.Value);
            }
            else
            {
                PizzaIDs.Add(cartitems[i].PizzaID.Value);
                // if its a pizza in this cart item we need to get the toppings for it from the carttoppings
                var toppingitems = databaseContext.CartToppings.Select(x => x)
                    .AsNoTracking()
                    .Where(x => x.CartID == cartitems[i].Id).ToList();
                List<int> ToppingId = new List<int>();
                for(int j=0;j<toppingitems.Count();j++)
                {
                    ToppingId.Add(toppingitems[j].ToppingId);
                }
                ToppingIDs.Add(ToppingId);
            }
        }
        // TODO: remove the cart items from the cart table
        CreateOrderModel createOrder = new CreateOrderModel
        {
            CustomerId=request.CustomerId.Value,
            PizzaIds=PizzaIDs,
            SideIds=SideIDs,
            ToppingIds=ToppingIDs,
            Status=Common.Enums.OrderStatus.Placed,
        };
        var order = createOrder.Map();
        databaseContext.Orders.Add(order);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);

        if(result > 0)
        {
            for(int i=0;i<cartitems.Count;i++)
            {
                if (cartitems[i].PizzaID!=null)
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
        for (int i = 0; i < createOrder.PizzaIds.Count; i++)
        {
            var OrderPizzaEntity = new OrderPizza
            {
                OrderId = LastOrderId,
                PizzaId = createOrder.PizzaIds[i],
            };

            databaseContext.OrderPizzas.Add(OrderPizzaEntity);
            await databaseContext.SaveChangesAsync(cancellationToken);
            // Add the toppings for this pizza in the orderpizzatoppings
            for (int j = 0; j < createOrder.ToppingIds[i].Count; j++)
            {
                //check if the toppings exisit
                databaseContext.OrderPizzaToppings.Add(new OrderPizzaTopping
                {
                    OrderPizzaId = OrderPizzaEntity.Id,
                    ToppingId = createOrder.ToppingIds[i][j],
                });
                await databaseContext.SaveChangesAsync(cancellationToken);
            }
        }

        await mediator.Publish(new OrderEvent { Data = createOrder }, cancellationToken);

        return Result.Success();
    }
}

// TODO: call a roll back if we cant sucefully insert the order into the db if forexample an incorrect entry has been made