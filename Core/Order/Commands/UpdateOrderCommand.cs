// TODO: Later Add the abillity to send emails everytime the status of the order is updated Maybe adding onto the notify table
// TODO: Possibily add on the ability for a specials by adding a special table to link specials

using Common.Models.Order;
using Common.Models.OrderModel;
using MediatR;

namespace Core.Order.Commands
{

    public class UpdateOrderCommand : IRequest<Result<OrderStatusModel>>
    {
        public UpdateOrderModel? Data { get; set; }

        public int? Id { get; set; }
    }

    public class UpdateOrderCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateOrderCommand, Result<OrderStatusModel>>
    {
        public async Task<Result<OrderStatusModel>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Data == null || request.Id == null)
            {
                return Result<OrderStatusModel>.Failure("Not Found");
            }
            var model = request.Data;
            var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Orders.FirstOrDefault(c => c.Id == id));
            var entity = await query(databaseContext, request.Id.Value);
            if (entity == null)
            {
                return Result<OrderStatusModel>.Failure("Not Found");
            }

            entity.Status = model.status;
            var outcome = databaseContext.Orders.Update(entity);
            var result = await databaseContext.SaveChangesAsync(cancellationToken);
            return result > 0 ? Result<OrderStatusModel>.Success(new OrderStatusModel
            {
                Id = entity.Id,
                Status = model.status,
                CreatedDate = entity.DateCreated.Value,
            }) : Result<OrderStatusModel>.Failure("Error");
        }
    }
}
