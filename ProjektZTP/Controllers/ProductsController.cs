using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Data;
using ProjektZTP.Features.ProductFeatures.Commands;
using ProjektZTP.Features.ProductFeatures.Queries;
using ProjektZTP.Features.UserFeatures.Commands;
using ProjektZTP.Models;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //in progress
        [HttpGet]
        public async Task<ActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var query = new GetProducts.Query();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        //done
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetProduct(Guid id)
        {
            var query = new GetProduct.Query(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        //done
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Edit(Guid id, EditProduct.EditData data, CancellationToken cancellationToken)
        {
            var command = new EditProduct.Command(id, data.Name, data.Price, data.Amount, data.Vat);
            EditProduct.Result product = await _mediator.Send(command, cancellationToken);
            return Ok("Product " + product.Name + "is edited");
        }

        //done
        [HttpPost]
        public async Task<ActionResult<Product>> Add(AddProduct.Command command, CancellationToken cancellationToken)
        {
            AddProduct.Result result = await _mediator.Send(command, cancellationToken);
            return Ok(new { objectName = result.id });
        }

        //done
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteProduct.Command(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
