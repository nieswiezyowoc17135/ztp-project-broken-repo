using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.ProductFeatures.Queries;

public class GetProduct
{
    public record GetProductQuery(Guid Id) : IRequest<GetProductResult>;

    public class GetProductHandler : IHandler<GetProductQuery, GetProductResult>
    {
        private readonly IProductsRepository _repository;

        public GetProductHandler(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductResult> HandleAsync(GetProductQuery request)
        {
            var product = await _repository.Get(request.Id);
            return new GetProductResult(
                product.Name,
                product.Price,
                product.Amount,
                product.Vat);
        }
    }

    public record GetProductResult(
        string Name,
        float Price,
        int Amount,
        float Vat);
}

