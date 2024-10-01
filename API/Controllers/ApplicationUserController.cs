using Common.Models.ApplicationUser;
using Common.Models.Order;
using Core.Customer.Commands;
using Core.Customer.Queries;
using Core.Order.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationUserController() : ApiController
{

    [Authorize]
    [HttpGet("{Email}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    // TODO: change the value we use to query
    public async Task<ActionResult> Get(string Email)
    {
        var result = await this.Mediator.Send(new GetApplicationUserQuery {Email = Email });
        return ResponseHelper.ResponseOutcome(result, this);
    }
    /// <summary>
    /// Get all Users.
    /// </summary>
    /// <returns>A <see cref="Task"/> repres
    /// enting the asynchronous operation.</returns>
    /// <response code="200">Customer Search</response>
    /// <response code="400">Error searching for customers</response>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ListResult<ApplicationUserModel>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [Route("Search")]
    public async Task<ActionResult> Search(SearchApplicationUserModel data)
    {
        var result = await this.Mediator.Send(new GetApplicationUsersQuery()
        {
            Data = data
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [Authorize]
    [HttpPut("Update/{email}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> UpdateApplicationUser([FromBody] UpdateApplicationUserModel? model,string? email)
    {
        var result = await this.Mediator.Send(new UpdateApplicationUserCommand() {
         Data=model,
         UserEmailAdress=email,
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [Authorize]
    [HttpDelete("{email}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> DeleteApplicationUser(string email)
    {
        var result = await this.Mediator.Send(new DeleteApplicationUserCommand() {
            UserEmail = email,
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }

    [HttpPost("AddCustomer")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> AddCustomerApplicationUser([FromBody] CreateApplicationUserModel model)
    {
        var result = await this.Mediator.Send(new CreateApplicationUserCommand()
        {
            Data = model,
            userClaim = new Claim("Role", "Customer")
        });
        return ResponseHelper.ResponseOutcome(result, this);
    }
    [Authorize(Policy ="AdminPolicy")]
    [HttpPost("AddStaff")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> AddStaffApplicationUser([FromBody] CreateApplicationUserModel model)
    {
        var result = await this.Mediator.Send(new CreateApplicationUserCommand()
        {
            Data = model,
            userClaim = new Claim("Role", "Staff")
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
    [Authorize]
    [HttpGet("{Email}/Orders")]
    [ProducesResponseType(typeof(ListResult<OrderModel>), 200)]
    [ProducesResponseType(typeof(ErrorResult), 400)]
    [ProducesResponseType(typeof(ErrorResult), 404)]
    public async Task<ActionResult> GetOrders(string Email)
    {
        var result = await this.Mediator.Send(new GetOrdersQuery { CustomerEmail = Email });
        return ResponseHelper.ResponseOutcome(result, this);
    }
}

