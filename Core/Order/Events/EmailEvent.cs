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
            pizzasContent.AppendLine($"<strong>{pizzaentity.Name}</strong> - {pizzaentity.Description}<br/>");
        }

        html = html.Replace("%pizzas%", pizzasContent.ToString());

        // Add order to data base
        databaseContext.Orders.Add(notification.Data.Map());

        databaseContext.Notifies.Add(new Notify
        {
            CustomerId = entity.Id,
            CustomerEmail = entity.Email,
            DateSent = null,
            EmailContent = html,
            Sent = false,
            Customer=entity
        });

        // EmailService emailService= new EmailService()
        // {
        //    Customer = entity.Map(),
        //    HtmlContent=html,

        // };
        // emailService.SendEmail();
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}