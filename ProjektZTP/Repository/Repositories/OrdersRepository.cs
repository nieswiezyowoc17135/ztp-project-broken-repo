using Microsoft.Identity.Client;
using ProjektZTP.Data;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Repository.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly DatabaseContext _context;

    public OrdersRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Add(Order orderEntry, CancellationToken cancellationToken)
    {
        if (orderEntry == null)
        {
            throw new ArgumentNullException(nameof(orderEntry));
        }

        await _context.Orders.AddAsync(orderEntry, cancellationToken);
        await Save(cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

