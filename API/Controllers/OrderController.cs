namespace Api.Controllers;
// TODO : Add validations for the sides
using Api.Helpers;
using Common.Models.Order;
using Common.Models.OrderModel;
using Core.Order.Commands;
using Core.Order.Queries;
using DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class OrderController() : ApiController
{
    /// <summary>
    /// Order Pizza.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Order
    ///     {
    ///       "customerId": 1,
    ///       "pizzaIds": [1, 2, 3, 4, 5]
    ///     }
    /// </remarks>
    /// <param name="model">Create Order Model</param>
    /// <returns>ActionResult</returns>
    [Authorize(Policy = "CustomerPolicy")]
    [HttpPost("{Email}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CreateOrderModel>> Create(string Email)
    {
        var result = await this.Mediator.Send(new OrderCommand
        {
            CustomerEmail = Email,
        });

        return ResponseHelper.ResponseOutcome(result, this);
    }


    /// <summary>
    /// Update Pizza Status.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     Patch /Update/{id}
    ///     {
    ///       "Id": id,
    ///       "OrderStatus": Accepted
    ///     }
    /// </remarks>
    /// <param name="model">Update Order Model</param>
    /// <returns>ActionResult</returns>
    [Authorize(Policy = "CustomerPolicy")]
    [HttpPatch("Update/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<UpdateOrderCommand>> Update([FromBody] UpdateOrderModel model, int id)
    {
        var result = await Mediator.Send(new UpdateOrderCommand
        {
            Id = id,
            Data = model
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }
    [Authorize(Policy = "CustomerPolicy")]
    [HttpPost("Search")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<SearchOrdersQuery>> Search([FromBody] SearchOrderModel model)
    {
        var result = await Mediator.Send(new SearchOrdersQuery
        {
            data = model
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [Authorize(Policy = "CustomerPolicy")]
    [HttpGet("NotCompleted/{customeremail}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GetNotCompletedOrders>> NotCompleted(string customeremail)
    {
        var result = await Mediator.Send(new GetNotCompletedOrders
        {
            CustomerEmail = customeremail
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [Authorize(Policy = "CustomerPolicy")]
    [HttpGet("Completed/{customeremail}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GetNotCompletedOrders>> Completed(string customeremail)
    {
        var result = await Mediator.Send(new GetCompletedOrders
        {
            CustomerEmail = customeremail
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [Authorize(Policy = "StaffPolicy")]
    [HttpPost("AllOrders")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GetOrdersStaff>> AllOrders([FromBody] StaffOrderModel data)
    {
        var result = await Mediator.Send(new GetOrdersStaff
        {
            Data = data
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}