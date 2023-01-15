﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using ProjektZTP.Features.OrderFeatures.Commands;
using ProjektZTP.Features.OrderFeatures.Queries;
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
        
        //done
        [HttpGet]
        public async Task<ActionResult> GetOrders(CancellationToken cancelaToken)
        {
            var query = new GetOrders.Query();
            var result = await _mediator.Send(query, cancelaToken);
            return Ok(result);
        }

        //done
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var query = new GetOrder.Query(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        //in progress
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(Guid id, EditOrder.EditData data, CancellationToken cancellationToken)
        {
            var command = new EditOrder.Command(id, data.Address, data.Customer);
            EditOrder.Result order = await _mediator.Send(command, cancellationToken);
            return Ok("Order " + order.Id + " is edited");
        }

        //Done
        [HttpPost]
        public async Task<ActionResult> Add(AddOrder.Command command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command,cancellationToken);
            return Ok(result.id);
        }
        
        //Done
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteOrder.Command(id);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

    }
}