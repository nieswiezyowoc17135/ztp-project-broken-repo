using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IProductsRepository
{
    Task Add(Product productEntry);
    Task Delete(Product product);
    Task<Product> Update(Product product);
    Task<Product> Get(Guid id);
    Task<IEnumerable<Product>> GetProducts();
}

