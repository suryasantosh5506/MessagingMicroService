using System.Data;

namespace MessagingMicroservice.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<IDbTransaction> BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}