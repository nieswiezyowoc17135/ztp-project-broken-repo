using System.Runtime.CompilerServices;
using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.ProductFeatures.Queries;

public class GetProducts
{
    public record Query() : IRequest<Result>;

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IProductsRepository _repository;

        public Handler (IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var tmpList = new List<ProductDTO>();
            var result = await _repository.GetProducts(cancellationToken);
            foreach (var product in result)
            {
                var tmp = new ProductDTO(
                    product.Name,
                    product.Price,
                    product.Amount,
                    product.Vat);
                tmpList.Add(tmp);
            }

            return new Result(tmpList);
        }
    }

    public record Result(
        List<ProductDTO> Products);

    public record ProductDTO(
        string Name,
        float Price,
        int Amount,
        float Vat);
}

