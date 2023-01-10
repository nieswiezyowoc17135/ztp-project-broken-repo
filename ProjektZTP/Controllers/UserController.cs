using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Features.UserFeatures.Commands;
using ProjektZTP.Features.UserFeatures.Queries;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var query = new GetUsers.Query();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetUser.Query(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Edit(Guid id, EditUser.EditData data, CancellationToken cancellationToken)
        {
            var command = new EditUser.Command(id, data.Login, data.Password, data.Email, data.FirstName,
                data.LastName);
            EditUser.Result user = await _mediator.Send(command, cancellationToken);
            return Ok($"User"+ user.Login + " is edited");
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddUser.Command command, CancellationToken cancellationToken)
        {
            AddUser.Result result = await _mediator.Send(command, cancellationToken);
            return Ok(new { objectName = result.Id });
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteUser.Command(id);
            DeleteUser.Result result = await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
