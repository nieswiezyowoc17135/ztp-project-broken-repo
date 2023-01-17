using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Features.UserFeatures.Commands;
using ProjektZTP.Features.UserFeatures.Queries;
using ProjektZTP.Mediator;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MediatorPattern.IMediator _mediator;

        public UserController(MediatorPattern.IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var query = new GetUsers.GetUsersQuery();
            var result = await _mediator.SendAsync(query);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetUserById(Guid id)
        {
            var query = new GetUser.GetUserQuery(id);
            var result = await _mediator.SendAsync(query);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Edit(Guid id, EditUser.EditUserCommand data)
        {
            var command = new EditUser.EditUserCommand(id, data.Login, data.Password, data.Email, data.FirstName,
                data.LastName);

            var user = await _mediator.SendAsync(command);

            return Ok($"User"+ user.Login + " is edited");
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddUser.AddUserCommand command)
        {
            var result = await _mediator.SendAsync(command);

            return Ok(new { objectName = result.Id });
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteUser.DeleteUserCommand(id);

            var result = await _mediator.SendAsync(command);

            return NoContent();
        }
    }
}
