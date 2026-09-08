using System.Data;
using System.Data.Common;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Infrastructure.Data;

namespace MessagingMicroservice.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DapperContext _dapperContext;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;
    public UnitOfWork(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IDbTransaction> BeginTransactionAsync()
    {
        _connection = _dapperContext.GetConnection();
        if (_connection.State != ConnectionState.Open)
            await ((DbConnection)_connection).OpenAsync();
        _transaction = await ((DbConnection)_connection).BeginTransactionAsync();
        return _transaction;
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await ((DbTransaction)_transaction).CommitAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await ((DbTransaction)_transaction).RollbackAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _transaction?.Dispose();
        _connection?.Dispose();
        _disposed = true;
    }
}