using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Repositories.Interfaces;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    ICustomerRepository Customers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}