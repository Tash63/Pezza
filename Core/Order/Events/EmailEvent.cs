namespace Core.Order.Events;

using System.Text;
using Common;
using Common.Entities;
using Common.Models.Order;
using Core.Email;
using DataAccess;
using MediatR;

public class OrderEvent : INotification
{
    public CreateOrderModel Data { get; set; }
}

public class OrderEventHandler(DatabaseContext databaseContext) : INotificationHandler<OrderEvent>
{
    async Task INotificationHandler<OrderEvent>.Handle(OrderEvent notification, CancellationToken cancellationToken)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\OrderCompleted.html";
        var html = File.ReadAllText(path);
        // find the customer with the specified ID
        var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
        var entity = await query(databaseContext, notification.Data.CustomerId);
        html = html.Replace("%name%", Convert.ToString(entity.Name));

        var pizzasContent = new StringBuilder();
        //get pizzas
        for (int i=0;i<notification.Data.PizzaIds.Count;i++)
        {
            var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
            var pizzaentity = await pizzaquery(databaseContext, notification.Data.PizzaIds.ElementAt(i));
            if(pizzaentity!=null)
            {
                pizzasContent.AppendLine($"<strong>{pizzaentity.Name}</strong> - {pizzaentity.Description}<br/>");
            }
            // TODO: sort out the exception if an invalid side is entered
        }
          
        for(int i=0;i<notification.Data.SideIds.Count;i++)
        {
            var sidequery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Sides.FirstOrDefault(s => s.ID == id));
            var side_entity = await sidequery(databaseContext,notification.Data.SideIds.ElementAt(i));
            if(side_entity!=null)
            {
                // TODO : Modify the email body to allow for the additon of sides
            }
        }

        html = html.Replace("%pizzas%", pizzasContent.ToString());

        // Add order to data base
        var orderesult = notification.Data.Map();
        databaseContext.Orders.Add(orderesult);

        databaseContext.Notifies.Add(new Notify
        {
            CustomerId = entity.Id,
            CustomerEmail = entity.Email,
            DateSent = null,
            EmailContent = html,
            Sent = false,
            Customer=entity
        });

        await databaseContext.SaveChangesAsync(cancellationToken);

        // Add Pizza's to that order
        int LastOrderId=orderesult.Id;
        // TODO: issue is coming from instation
        for(int i=0;i<notification.Data.PizzaIds.Count;i++)
        {
            var OrderPizzaEntity = new OrderPizza
            {
                OrderId = LastOrderId,
                PizzaId = notification.Data.PizzaIds[i],
            };
            databaseContext.OrderPizzas.Add(OrderPizzaEntity);
            await databaseContext.SaveChangesAsync(cancellationToken);
            for(int j = 0; j < notification.Data.ToppingIds[i].Count;j++)
            {
                databaseContext.OrderPizzaToppings.Add(new OrderPizzaTopping
                {
                    OrderPizzaId = OrderPizzaEntity.Id,
                    ToppingId = notification.Data.ToppingIds[i][j],
                });
                await databaseContext.SaveChangesAsync(cancellationToken);
            }
        }
        // TODO : Modify the create order field to add a parllell list of list of topping ids
        // save changes

    }
}