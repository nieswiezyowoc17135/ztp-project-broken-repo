using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IOrdersRepository
{
    Task Add(Order orderEntry, CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
}

