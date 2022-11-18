using PaymentsTracker.Models.Data;
using PaymentsTracker.Models.Models;
using PaymentsTracker.Repositories.Interfaces;

namespace PaymentsTracker.Repositories.Business;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IRepository<User> Users => new Repository<User>(_dbContext);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}