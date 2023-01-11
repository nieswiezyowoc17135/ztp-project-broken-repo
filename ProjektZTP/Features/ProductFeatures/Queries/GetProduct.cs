using MediatR;
using NuGet.Versioning;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.ProductFeatures.Queries;

public class GetProduct
{
    public record Query(Guid Id) : IRequest<Result>;

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IProductsRepository _repository;

        public Handler(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await _repository.Get(request.Id, cancellationToken);
            return new Result(
                product.Name,
                product.Price,
                product.Amount,
                product.Vat);
        }
    }

    public record Result(
        string Name,
        float Price,
        int Amount,
        float Vat);
}

