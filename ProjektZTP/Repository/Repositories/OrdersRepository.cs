using Microsoft.EntityFrameworkCore;
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

    public async Task Delete(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Remove(order);
        await Save(cancellationToken);
    }

    public async Task<Order> Update(Order order, CancellationToken cancellationToken)
    {
        await Save(cancellationToken);
        return order;
    }

    public async Task<Order> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _context.Orders.FindAsync(id, cancellationToken);
        if (result == null)
        {
            throw new Exception("There is no Order with this ID in database");
        }

        return result;
    }

    public async Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken)
    {
        IEnumerable<Order> orders = await _context.Orders
            .AsNoTracking()
            .Select(x => new Order(
                x.Id,
                x.Customer,
                x.Address,
                x.UserId
            )).ToListAsync(cancellationToken);
        return orders;
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

