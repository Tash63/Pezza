namespace Core.Order.Events;

using System.Text;
using Common.Entities;
using Common.Models.Customer;
using Common.Models.Order;
using Core.Customer.Queries;
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
        //get pizza and customer entitys
        var customerquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == notification.Data.CustomerId));
        var customerentity = await customerquery(databaseContext, notification.Data.CustomerId);
        Customer customer=customerentity;

        html = html.Replace("%name%", Convert.ToString(customer.Name));
        List<int> pizzaids=notification.Data.PizzaIds.ToList();
        var pizzasContent = new StringBuilder();
        for (int i=0;i<pizzaids.Count;i++)
        {
            //get each pizza model and append to line
             var pizzaquery = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
            var pizzaentity = await pizzaquery(databaseContext, pizzaids.ElementAt(i));
            pizzasContent.AppendLine($"<strong>{pizzaentity.Name}</strong> - {pizzaentity.Description}<br/>");
        }

        html = html.Replace("%pizzas%", pizzasContent.ToString());

        databaseContext.Orders.Add(notification.Data);

        databaseContext.Notifies.Add(new Notify
        {
            CustomerId = customer.Id,
            CustomerEmail = customer.Name,
            DateSent = DateTime.Now,
            EmailContent = html,
            Sent = false
        });

        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}