using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Features.OrderFeatures.Commands;
using ProjektZTP.Features.OrderFeatures.Queries;
using ProjektZTP.Mediator;
using ProjektZTP.Models;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MediatorPattern.IMediator _mediator;

        public OrdersController(MediatorPattern.IMediator customMediator)
        {
            _mediator = customMediator;
        }

        //done
        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {
            var query = new GetOrders.GetOrdersQuery();
            var result = await _mediator.SendAsync(query);
            return Ok(result);
        }

        //done
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var query = new GetOrder.GetOrderQuery(id);
            var result = await _mediator.SendAsync(query);
            return Ok(result);
        }

        //in progress
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(Guid id, EditOrder.EditOrderCommand data)
        {
            var command = new EditOrder.EditOrderCommand(id, data.Address, data.Customer);

            var order = await _mediator.SendAsync(command);
            return Ok("Order " + order.Id + " is edited");
        }

        //Done
        [HttpPost]
        public async Task<ActionResult> Add(AddOrder.AddOrderCommand command)
        {
            var result = await _mediator.SendAsync(command);

            return Ok();
        }
        
        //Done
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteOrder.DeleteOrderCommand(id);
            await _mediator.SendAsync(command);
            return Ok();
        }

    }
}
