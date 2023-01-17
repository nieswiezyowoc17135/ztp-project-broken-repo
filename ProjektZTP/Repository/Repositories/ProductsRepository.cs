using Microsoft.EntityFrameworkCore;
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
    public async Task Add(Product productEntry)
    {
        if (productEntry == null)
        {
            throw new ArgumentNullException(nameof(productEntry));
        }

        await _context.Products.AddAsync(productEntry);
        await Save();
    }

    //done
    public async Task Delete(Product product)
    {
        _context.Products.Remove(product);
        await Save();
    }

    //done
    public async Task<Product> Update(Product product)
    {
        await Save();
        return product;
    }

    //done
    public async Task<Product> Get(Guid id)
    {
        var product = await _context.Products.FindAsync(new object[] { id });
        if (product == null)
        {
            throw new Exception("There is no Product with this ID in database");
        }

        return product;
    }

    //done
    public async Task<IEnumerable<Product>> GetProducts()
    {
        IEnumerable<Product> products = await _context.Products
            .AsNoTracking()
            .Select(x => new Product(
                x.Name,
                x.Price,
                x.Amount,
                x.Vat)).ToListAsync();
        return products;
    }

    //done
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}

