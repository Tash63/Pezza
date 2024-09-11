using Common.Models.Customer;
using MediatR;

namespace Core.Customer.Commands;

public class UpdateCustomerCommand : IRequest<Result<CustomerModel>>
{
    public int? Id { get; set; }

    public UpdateCustomerModel? Data { get; set; }
}

public class UpdateCustomerCommandHandler(DatabaseContext databaseContext) : IRequestHandler<UpdateCustomerCommand, Result<CustomerModel>>
{
    public async Task<Result<CustomerModel>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (request.Data == null || request.Id == null)
        {
            return Result<CustomerModel>.Failure("Error");
        }

        var model = request.Data;
        var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
        //this is getting the acutal entry into thhe database so we can modify what needs to
        var findEntity = await query(databaseContext, request.Id.Value);
        if (findEntity == null)
        {
            return Result<CustomerModel>.Failure("Not found");
        }

        findEntity.Name=string.IsNullOrEmpty(model.Name)?findEntity.Name:model.Name;
        findEntity.Address=string.IsNullOrEmpty(model.Address)?findEntity.Address:model.Address;
        findEntity.Email=string.IsNullOrEmpty(model.Email)?findEntity.Email:model.Email;
        findEntity.Cellphone=string.IsNullOrEmpty(model.Cellphone)?findEntity.Cellphone:model.Cellphone;

        var outcome = databaseContext.Customers.Update(findEntity);
        var result = await databaseContext.SaveChangesAsync(cancellationToken);

        return result > 0 ? Result<CustomerModel>.Success(findEntity.Map()) : Result<CustomerModel>.Failure("Error");
    }
}