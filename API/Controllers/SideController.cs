using Common.Models.Side;
using Core.Side.Command;
using Core.Side.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //Annotations to sepcify this is a controller
    [ApiController]
    [Route("[controller]")]
    public class SideController : ApiController
    {

        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this.Mediator.Send(new GetSideQuery { ID = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [Authorize(Policy = "StaffPolicy")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> remove(int id)
        {
            var result = await this.Mediator.Send(new DeleteSideCommand { Id = id });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost("Search")]
        [ProducesResponseType(typeof(ListResult<SideModel>), 200)]
        [ProducesResponseType(typeof(ErrorResult), 400)]
        public async Task<ActionResult> Search(SearchSideModel data)
        {
            var result = await this.Mediator.Send(new GetSideQuerys()
            {
                Data = data
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }

        [Authorize(Policy = "StaffPolicy")]
        [HttpPost("Add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Add(CreateSideModel data)
        {
            var result = await this.Mediator.Send(new CreateSideCommand()
            {
                Data = data
            });
            return ResponseHelper.ResponseOutcome(result,this);
        }

        [Authorize(Policy = "StaffPolicy")]
        [HttpPut("Update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Update(UpdateSideModel data,int id)
        {
            var result = await this.Mediator.Send(new UpdateSideCommand()
            {
                Data = data,
                Id = id
            });
            return ResponseHelper.ResponseOutcome(result, this);
        }
    }
}
