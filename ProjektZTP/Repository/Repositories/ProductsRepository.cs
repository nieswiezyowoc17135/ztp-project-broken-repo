using ProjektZTP.Data;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Repository.Repositories;

public class ProductsRepository : IProductsRepository
{
    public readonly DatabaseContext _context;

    public ProductsRepository(DatabaseContext context)
    {
        _context = context;
    }

    //done
    public async Task Add(Product productEntry, CancellationToken cancellationToken)
    {
        if (productEntry == null)
        {
            throw new ArgumentNullException(nameof(productEntry));
        }

        await _context.Products.AddAsync(productEntry, cancellationToken);
        await Save(cancellationToken);
    }

    //done
    public async Task Delete(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Remove(product);
        await Save(cancellationToken);
    }

    //done
    public async Task<Product> Update(Product product, CancellationToken cancellationToken)
    {
        await Save(cancellationToken);
        return product;
    }

    //done
    public async Task<Product> Get(Guid id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product == null)
        {
            throw new Exception("There is no Product with this ID in database");
        }

        return product;
    }

    public Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

