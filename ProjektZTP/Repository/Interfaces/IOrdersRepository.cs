using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IOrdersRepository
{
    Task Add(Order orderEntry);
    Task Delete(Order order);
    Task<Order> Update(Order order);
    Task<Order>  Get(Guid id);
    Task<IEnumerable<Order>> GetOrders();
    Task Save();
}

