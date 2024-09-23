using Common.Models.Cart;
using Core.Cart.Command;
using Core.Customer.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ApiController
    {
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

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RemoveFromCart>>RemoveItem(int? id)
        {
            var result=await Mediator.Send(new RemoveFromCart { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [HttpGet("CustomerCart/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GetCartQuery>> CustomerCart(int? id)
        {
            var result = await Mediator.Send(new GetCartQuery { CustomerID = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
