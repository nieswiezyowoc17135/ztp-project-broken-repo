using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Features.UserFeatures.Commands;
using static ProjektZTP.Features.UserFeatures.Commands.EditUser;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator=mediator;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            throw new NotImplementedException();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(Guid id, EditUser.InputData data, CancellationToken cancellationToken)
        {
            var temp = new EditUser.Command(id, data.Login, data.Password, data.Email, data.FirstName,
                data.LastName);
            EditUser.Result user = await _mediator.Send(temp, cancellationToken);
           return Ok(new { objectName = user.FirstName + " user is edited"});
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> Add(AddUser.Command command, CancellationToken cancellationToken)
        {
            AddUser.Result result = await _mediator.Send(command, cancellationToken);
            return Ok(new { objectName = result.Id });
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }
}
