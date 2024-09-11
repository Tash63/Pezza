namespace Test.Core;

using Common.Models.Customer;
using global::Core.Customer.Commands;
using global::Core.Customer.Queries;
using global::Core.Pizza.Queries;
using Test.Setup.TestData.Customer;
using static global::Core.Customer.Commands.CreateCustomerCommand;
using static global::Core.Customer.Commands.DeleteCustomerCommand;
using static global::Core.Customer.Commands.UpdateCustomerCommand;


[TestFixture]
public class TestCustomerCore : QueryTestBase
{
    private CustomerModel model;

    [SetUp]
    public async Task Init()
    {
        this.model = CustomerTestData.CustomerModel;
        var sutCreate = new CreateCustomerCommandHandler(this.Context);
        var resultCreate = await sutCreate.Handle(
            new CreateCustomerCommand
            {
                Data = new CreateCustomerModel
                {
                    Name = this.model.Name,
                    Email = this.model.Email,
                    Address = this.model.Address,
                    Cellphone = this.model.Cellphone
                }
            }, CancellationToken.None);

        if (!resultCreate.Succeeded)
        {
            Assert.IsTrue(false);
        }

        this.model = resultCreate.Data;
    }

    [Test]
    public async Task GetAsync()
    {
        var sutGet = new GetCustomerQueryHandler(this.Context);
        var resultGet = await sutGet.Handle(
            new GetCustomerQuery
            {
                Id = this.model.Id
            }, CancellationToken.None);

        Assert.IsTrue(resultGet?.Data != null);
    }

    [Test]
    public async Task GetAllByAdresssync()
    {
        var sutGetAll = new GetCustomersQueryHandler(this.Context);
        var resultGetAll = await sutGetAll.Handle(new GetCustomersQuery
        {
            Data = new SearchCustomerModel
            {
                OrderBy = null,
                PagingArgs = new Common.Models.PagingArgs
                {
                    Offset = 0,
                    Limit = 0,
                    UsePaging = true
                },
                Id = 0,
                Name = "",
                Address = this.model.Address,
                Email = "",
                Cellphone = "",
                DateCreated = this.model.DateCreated
            }
        }, CancellationToken.None);

        Assert.IsTrue(resultGetAll?.Data.Count == 1);
    }

 
    [Test]
    public void SaveAsync() => Assert.IsTrue(this.model != null);

    [Test]
    public async Task UpdateAsync()
    {
        var sutUpdate = new UpdateCustomerCommandHandler(this.Context);
        var resultUpdate = await sutUpdate.Handle(
            new UpdateCustomerCommand
            {
                Id = this.model.Id,
                Data = new UpdateCustomerModel
                {
                    Cellphone = "0721230000"
                }
            }, CancellationToken.None);

        Assert.IsTrue(resultUpdate.Succeeded);
    }

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteCustomerCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteCustomerCommand
            {
                Id = this.model.Id
            }, CancellationToken.None);

        Assert.IsTrue(outcomeDelete.Succeeded);
    }
}