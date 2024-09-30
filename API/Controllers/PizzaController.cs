namespace Api.Controllers;

using Api.Helpers;
using Common.Models.Pizza;
using Core.Pizza.Commands;
using Core.Pizza.Queries;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
public class PizzaController() : ApiController
{
    /// <summary>
    /// Get Pizza by Id.
    /// </summary>
    /// <param name="id">Pizza Id</param>
    /// <returns>ActionResult</returns>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Get(int id)
    {
        var result = await this.Mediator.Send(new GetPizzaQuery { Id = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Get all Pizzas.
    /// </summary>
    /// <returns>ActionResult</returns>
    [Authorize]
    [HttpPost("Search")]
    [ProducesResponseType(typeof(ListResult<PizzaModel>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    //whats in gere is required by the request
    public async Task<ActionResult> Search(SearchPizzaModel data)
    {
        var result = await this.Mediator.Send(new GetPizzasQuery() { Data=data});
        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Create Pizza.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST api/Pizza
    ///     {
    ///       "name": "Hawaiian",
    ///       "description": "Hawaiian pizza is a pizza originating in Canada, and is traditionally topped with pineapple, tomato sauce, cheese, and either ham or bacon.",
    ///       "price": "99"
    ///     }
    /// </remarks>
    /// <param name="model">Pizza Model</param>
    /// <returns>ActionResult</returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Pizza>> Create([FromBody] CreatePizzaModel model)
    {
        var result = await this.Mediator.Send(new CreatePizzaCommand
        {
            Data = model
        });

        return ResponseHelper.ResponseOutcome(result, this);
    }


    /// <summary>
    /// Update Pizza.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT api/Pizza/1
    ///     {
    ///       "price": "119"
    ///     }
    /// </remarks>
    /// <param name="model">Pizza Model</param>
    /// <returns>ActionResult</returns>
    [Authorize]
    [HttpPut("{Pizzaid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Update([FromBody] UpdatePizzaModel model,int Pizzaid)
    {
        var result = await this.Mediator.Send(new UpdatePizzaCommand
        {
            Data = model,
            Id = Pizzaid,
        });

        return ResponseHelper.ResponseOutcome(result, this);
    }

    /// <summary>
    /// Delete Pizza by Id.
    /// </summary>
    /// <param name="id">Pizza Id</param>
    /// <returns>ActionResult</returns>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await this.Mediator.Send(new DeletePizzaCommand { Id = id });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}