﻿using Api.Helpers;
using Common.Models.Topping;
using Core.Topping.Command;
using Core.Topping.Query;
using MediatR;
// TODO: Documentation
namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToppingController : ApiController
    {
        [HttpGet("/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetToppingQuery { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [HttpPost("Search")]
        [ProducesResponseType(typeof(ListResult<ToppingModel>),200)]
        [ProducesResponseType(typeof(ErrorResult),400)]
        public async Task<ActionResult> Search(SearchToppingModel data)
        {
            var result = await this.Mediator.Send(new GetToppingsQuery()
            {
                Data = data
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreateToppingModel>> Create([FromBody] CreateToppingModel model)
        {
            var result = await this.Mediator.Send(new CreateToppingCommand
            {
                data = model
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public async Task<ActionResult> Update([FromBody] UpdateToppingModel model,int id)
        {
            var result = await this.Mediator.Send(new UpdateToppingCommand
            {
                Data = model,
                Id = id
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.Mediator.Send(new DeleteToppingCommand
            {
                Id = id
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}