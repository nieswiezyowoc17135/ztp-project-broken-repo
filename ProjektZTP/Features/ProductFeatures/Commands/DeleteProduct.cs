using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.ProductFeatures.Commands;

public class DeleteProduct
{
    public record DeleteProductCommand(
        Guid Id) : IRequest<DeleteProductResult>;

    public class DeleteOrderCommandHandler : IHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IProductsRepository _repository;
        private Logger _logger;
        
        
        public DeleteOrderCommandHandler(IProductsRepository repository)
        {
            _logger = Logger.GetInstance();
            _repository = repository;
        }

        public async Task<DeleteProductResult> HandleAsync(DeleteProductCommand request)
        {
            var productToDelete = await _repository.Get(request.Id);
            await _repository.Delete(productToDelete);
            _logger.Log("Product deleted.");
            return new DeleteProductResult(request.Id);
        }
    }

    public record DeleteProductResult(Guid Id);
}

