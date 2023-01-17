using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Data;
using ProjektZTP.Features.ProductFeatures.Commands;
using ProjektZTP.Features.ProductFeatures.Queries;
using ProjektZTP.Features.UserFeatures.Commands;
using ProjektZTP.Mediator;
using ProjektZTP.Models;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MediatorPattern.IMediator _mediator;

        public ProductsController(MediatorPattern.IMediator mediator)
        {
            _mediator = mediator;
        }

        //in progress
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var query = new GetProducts.GetProdcutsQuery();
            var result = await _mediator.SendAsync(query);
            return Ok(result);
        }

        //done
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetProduct(Guid id)
        {
            var query = new GetProduct.GetProductQuery(id);
            var result = await _mediator.SendAsync(query);

            return Ok(result);
        }

        //done
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Edit(Guid id, EditProduct.EditProductCommand data)
        {
            var command = new EditProduct.EditProductCommand(id, data.Name, data.Price, data.Amount, data.Vat);
            var product = await _mediator.SendAsync(command);

            return Ok("Product " + product.Name + "is edited");
        }

        //done
        [HttpPost]
        public async Task<ActionResult<Product>> Add(AddProduct.AddProductCommand command)
        {
            var result = await _mediator.SendAsync(command);
            return Ok(new { objectName = result.id });
        }

        //done
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteProduct.DeleteProductCommand(id);
            await _mediator.SendAsync(command);

            return NoContent();
        }
    }
}
