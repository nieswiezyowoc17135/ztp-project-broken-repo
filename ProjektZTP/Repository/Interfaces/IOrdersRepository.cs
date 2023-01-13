using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IOrdersRepository
{
    Task Add(Order orderEntry, CancellationToken cancellationToken);
    Task Delete(Order order, CancellationToken cancellationToken);
    Task<Order> Update(Order order, CancellationToken cancellationToken);
    Task<Order>  Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
}

