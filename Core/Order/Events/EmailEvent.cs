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
        var query = EF.CompileAsyncQuery((DatabaseContext db, string email) => db.Users.FirstOrDefault(c => c.Email == email));
        // TODO: fix this
        var entity = await query(databaseContext, notification.Data.UserEmail);
        html = html.Replace("%name%", Convert.ToString(entity.FullName));

        var pizzasContent = new StringBuilder();

        // get pizzas
        for (int i=0;i<notification.Data.PizzaIds.Count;i++)
        {
            var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
            var pizzaentity = await pizzaquery(databaseContext, notification.Data.PizzaIds.ElementAt(i));
            if(pizzaentity!=null)
            {
                pizzasContent.AppendLine($"<strong>{pizzaentity.Name}</strong> - {pizzaentity.Description}<br/>");
            }
            // TODO: move this to a seapreate file to insert into the db
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

        databaseContext.Notifies.Add(new Notify
        {
            DateSent = null,
            EmailContent = html,
            Sent = false,
            UserEmail=entity.Email,
        });

        await databaseContext.SaveChangesAsync(cancellationToken);

    }
}