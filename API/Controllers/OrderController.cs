﻿namespace Api.Controllers;
// TODO : Add validations for the sides
using Api.Helpers;
using Common.Models.Order;
using Common.Models.OrderModel;
using Core.Order.Commands;
using Core.Order.Queries;
using DataAccess;
using MediatR;
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
    [HttpPost("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CreateOrderModel>> Create(int id)
    {
        var result = await this.Mediator.Send(new OrderCommand
        {
            CustomerId =id,
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
    [HttpPatch("Update/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<UpdateOrderCommand>>Update([FromBody] UpdateOrderModel model,int id)
    {
        var result = await Mediator.Send(new UpdateOrderCommand
        {
            Id = id,
            Data = model
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpPost("Search")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]

    public async Task<ActionResult<SearchOrdersQuery>> Search([FromBody] SearchOrderModel model)
    {
        var result = await Mediator.Send(new SearchOrdersQuery
        {
            data=model
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}