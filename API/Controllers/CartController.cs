using Common.Models.Cart;
using Core.Cart.Command;
using Core.Customer.Queries;
using Core.Pizza.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ApiController
    {
        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AddToCart>> AddItem([FromBody] AddToCartModel model)
        {
            var result = await Mediator.Send(new AddToCart
            {
                Data = model
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RemoveFromCart>>RemoveItem(int? id)
        {
            var result=await Mediator.Send(new RemoveFromCart { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("CustomerCart/{UserEmail}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GetCartQuery>> CustomerCart(string UserEmail)
        {
            var result = await Mediator.Send(new GetCartQuery { CustomerEmail = UserEmail });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpPut("CustomerCart/{CartId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UpdateCartCommand>> UpdateCart([FromBody] UpdateCartModel model,int CartId)
        {
            var result = await Mediator.Send(new UpdateCartCommand
            {
                Id = CartId,
                Data=model
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
