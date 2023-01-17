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

    public async Task Add(Order orderEntry)
    {
        if (orderEntry == null)
        {
            throw new ArgumentNullException(nameof(orderEntry));
        }

        await _context.Orders.AddAsync(orderEntry);
        await Save();
    }

    public async Task Delete(Order order)
    {
        _context.Orders.Remove(order);
        await Save();
    }

    public async Task<Order> Update(Order order)
    {
        await Save();
        return order;
    }

    public async Task<Order> Get(Guid id)
    {
        var result = await _context.Orders.FindAsync(id);
        if (result == null)
        {
            throw new Exception("There is no Order with this ID in database");
        }

        return result;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        IEnumerable<Order> orders = await _context.Orders
            .AsNoTracking()
            .Select(x => new Order(
                x.Id,
                x.Customer,
                x.Address,
                x.UserId
            )).ToListAsync();
        return orders;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}

