using System.Runtime.CompilerServices;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.ProductFeatures.Queries;

public class GetProducts
{
    public record GetProdcutsQuery() : IRequest<GetProductsResult>;

    public class Handler : IHandler<GetProdcutsQuery, GetProductsResult>
    {
        private readonly IProductsRepository _repository;

        public Handler (IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductsResult> HandleAsync(GetProdcutsQuery request)
        {
            var tmpList = new List<ProductDTO>();
            var result = await _repository.GetProducts();
            foreach (var product in result)
            {
                var tmp = new ProductDTO(
                    product.Name,
                    product.Price,
                    product.Amount,
                    product.Vat);
                tmpList.Add(tmp);
            }

            return new GetProductsResult(tmpList);
        }
    }

    public record GetProductsResult(
        List<ProductDTO> Products);

    public record ProductDTO(
        string Name,
        float Price,
        int Amount,
        float Vat);
}

