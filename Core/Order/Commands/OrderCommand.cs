namespace Core.Order.Commands;

using Common.Entities;
using Common.Models.Order;
using Core.Order.Events;
using DataAccess;
using MediatR;

public class OrderCommand : IRequest<Result>
{
    public required CreateOrderModel Data { get; set; }
}

public class OrderCommandHandler(IMediator mediator,DatabaseContext databaseContext) : IRequestHandler<OrderCommand, Result>
{

    public async Task<Result> Handle(OrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            return Result.Failure("Error");
        }

        // validate the PizzaIds,sideids and topping ids
        for(int i=0;i<request.Data.PizzaIds.Count;i++)
        {
            var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db,int id)=>db.Pizzas.FirstOrDefault(x=>x.Id==id));
            var pizzaresult = await pizzaquery(databaseContext,request.Data.PizzaIds.ElementAt(i));
            if(pizzaresult==null)
            {
                return Result.Failure("Not Found");
            }
        } 

        for(int i=0;i<request.Data.SideIds.Count;i++)
        {
            var Sidequery = EF.CompileAsyncQuery((DatabaseContext db,int id)=>db.Sides.FirstOrDefault(x=>x.ID==id));
            var Sideresult = await Sidequery(databaseContext,request.Data.SideIds.ElementAt(i));
            if(Sideresult==null)
            {
                return Result.Failure("Not Found");
            }
        }
        
        for(int i=0;i<request.Data.ToppingIds.Count;i++)
        {
            for(int j=0;j<request.Data.ToppingIds.ElementAt(i).Count; j++)
            {
                var toppingquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Toppings.FirstOrDefault(x=>x.Id==id));
                var toppingresult = await toppingquery(databaseContext, request.Data.ToppingIds.ElementAt(i).ElementAt(j));
                if(toppingresult==null)
                {
                    return Result.Failure("Not Found");
                }
            }
        }


        var orderesult = request.Data.Map();
        databaseContext.Orders.Add(orderesult);
        await databaseContext.SaveChangesAsync(cancellationToken);
        // Add Pizza's to that order
        int LastOrderId = orderesult.Id;
        for (int i = 0; i < request.Data.PizzaIds.Count; i++)
        {
                var OrderPizzaEntity = new OrderPizza
                {
                    OrderId = LastOrderId,
                    PizzaId = request.Data.PizzaIds[i],
                };

                databaseContext.OrderPizzas.Add(OrderPizzaEntity);
                await databaseContext.SaveChangesAsync(cancellationToken);
                // Add the toppings for this pizza in the orderpizzatoppings
                for (int j = 0; j < request.Data.ToppingIds[i].Count; j++)
                {
                    //check if the toppings exisit
                        databaseContext.OrderPizzaToppings.Add(new OrderPizzaTopping
                        {
                            OrderPizzaId = OrderPizzaEntity.Id,
                            ToppingId = request.Data.ToppingIds[i][j],
                        });
                        await databaseContext.SaveChangesAsync(cancellationToken);
            }
        }
        await mediator.Publish(new OrderEvent { Data = request.Data }, cancellationToken);

        return Result.Success();
    }
}

// TODO: call a roll back if we cant sucessfully insert the order into the db if forexample an incorrect entry has been made