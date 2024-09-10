namespace Api.Controllers;

using Api.Helpers;
using Common.Models.Customer;
using Common.Models.Pizza;
using Core.Customer.Commands;
using Core.Customer.Queries;
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
    //define the route and endpoint so route is Customer and endpoint will be Customer/Search being a put trnasaction
    [HttpPost("Search")]
    [ProducesResponseType(200)]
    public async Task<ActionResult> Search()
    {
        //this will be used to get all the pizzas from the database
        var result = await this.Mediator.Send(new GetPizzaQuery());
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpPost("Add")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerModel model)
    {
        //The way the mediator works is by sending a mesage to a class to perfom a ceritain action in this case to the CreateCustomerCommand calss to intilize it 
        var result = await this.Mediator.Send(new CreateCustomerCommand
        {
            Data = model
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpPut("Update")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerModel model)
    {
        var result = await this.Mediator.Send(new UpdateCustomerCommand
        {
            Data = model
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}

