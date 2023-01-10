using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IProductsRepository
{
    Task Add(Product productEntry, CancellationToken cancellationToken);
    Task Delete(Product product, CancellationToken cancellationToken);
    Task<Product> Update(Product product, CancellationToken cancellationToken);
    Task<Product> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken);
}

