// TODO: Later Add the abillity to send emails everytime the status of the order is updated Maybe adding onto the notify table
// TODO: Possibily add on the ability for a specials by adding a special table to link specials

using Common.Models.Order;
using Common.Models.OrderModel;
using MediatR;

namespace Core.Order.Commands
{

    public class UpdateOrderCommand : IRequest<Result<CreateOrderModel>>
    {
        public UpdateOrderModel? Data { get; set; }

        public int? Id { get; set; }
    }

    public class UpdateOrderCommandHandler(DatabaseContext databaseContext): IRequestHandler<UpdateOrderCommand,Result<CreateOrderModel>>
    {
        public async Task<Result<CreateOrderModel>> Handle(UpdateOrderCommand request,CancellationToken cancellationToken)
        {
            if(request.Data==null ||request.Id==null)
            {
                return Result<CreateOrderModel>.Failure("Not Found");
            }
            var model=request.Data;
            var query = EF.CompileAsyncQuery((DatabaseContext db, int id) =>db.Orders.FirstOrDefault(c=>c.Id==id));
            var entity = await query(databaseContext, request.Id.Value);
            if (entity == null)
            {
                return Result<CreateOrderModel>.Failure("Not Found");
            }

            entity.Status = model.status;
            var outcome=databaseContext.Orders.Update(entity);
            var result=await databaseContext.SaveChangesAsync(cancellationToken);

            // get the Pizza List 
            List<int> PizzaIds = new List<int>();
            var pizzas=databaseContext.OrderPizzas
            .Select(x => x)
            .AsNoTracking()
            .Where(x=>x.OrderId==request.Id.Value).ToList();
            for(int i=0;i<pizzas.Count();i++)
            {
                PizzaIds.Add(pizzas.ElementAt(i).PizzaId);
            }
            CreateOrderModel resultModel=new CreateOrderModel()
            {
                PizzaIds = PizzaIds,
                SideIds=entity.SideIds,
                Status=entity.Status,
                UserEmail=entity.UserEmail,
            };
            return result > 0 ? Result<CreateOrderModel>.Success(resultModel) : Result<CreateOrderModel>.Failure("Error");
        }
    }
}
