namespace Api.Controllers;

using Api.Helpers;
using Common.Models.Customer;
using Common.Models.Order;
using Common.Models.Pizza;
using Core.Customer.Commands;
using Core.Customer.Queries;
using Core.Order.Queries;
using Core.Pizza.Commands;
using Core.Pizza.Queries;

[ApiController]
[Route("[controller]")]
public class CustomerController() : ApiController
{
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Get(int id)
    {
        var result = await this.Mediator.Send(new GetCustomerQuery { Id = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }
    /// <summary>
    /// Get all Customers.
    /// </summary>
    /// <returns>A <see cref="Task"/> repres
    /// enting the asynchronous operation.</returns>
    /// <response code="200">Customer Search</response>
    /// <response code="400">Error searching for customers</response>
    [HttpPost]
    [ProducesResponseType(typeof(ListResult<CustomerModel>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [Route("Search")]
    public async Task<ActionResult> Search(SearchCustomerModel data)
    {
        var result = await this.Mediator.Send(new GetCustomersQuery()
        {
            Data = data
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpPost("Add")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerModel model)
    {
        // The way the mediator works is by sending a mesage to a class to perfom a ceritain action in this case to the CreateCustomerCommand calss to intilize it 
        var result = await this.Mediator.Send(new CreateCustomerCommand
        {
            Data = model
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpPut("Update/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerModel model,int id)
    {
        var result = await this.Mediator.Send(new UpdateCustomerCommand
        {
            Data = model,
            Id = id
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var result = await this.Mediator.Send(new DeleteCustomerCommand
        {
            Id = id
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Get Customer Orders by Id.
    /// </summary>
    /// <param name="id">int.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <response code="200">Get customer orders</response>
    /// <response code="400">Error getting customer orders</response>
    /// <response code="404">Customer orders not found</response>
    [HttpGet("{id}/Orders")]
    [ProducesResponseType(typeof(ListResult<OrderModel>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [ProducesResponseType(typeof(ErrorResult), 404)]
    public async Task<ActionResult> GetOrders(int id)
    {
        var result = await this.Mediator.Send(new GetOrdersQuery { CustomerID = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }

}

