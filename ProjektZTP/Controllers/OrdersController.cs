using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Features.OrderFeatures.Commands;
using ProjektZTP.Models;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
/*
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            throw new NotImplementedException();
        }
*/

        //In progress
        [HttpPost]
        public async Task<ActionResult> Add(AddOrder.Command command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command,cancellationToken);
            return Ok(result.id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
