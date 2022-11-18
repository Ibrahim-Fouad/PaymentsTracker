using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Repositories.Interfaces;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}