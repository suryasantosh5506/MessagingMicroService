using Dapper;
using MessagingMicroservice.Domain.Enums;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Infrastructure.Data;

namespace MessagingMicroservice.Infrastructure.Repositories;

public class ProviderRepository:IProviderRepository
{

    private readonly DapperContext _dapperContext;
    
    public ProviderRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    public async Task<int> InsertProvider(MessageProvider provider)
    {
        string query = """
                        INSERT INTO dbo.tblProviders
                        (
                            MessageProvider
                        )
                        OUTPUT INSERTED.Id
                        VALUES
                        (
                            @MessageProvider
                        );
                       """;

        using var connection = _dapperContext.GetConnection();

        return await connection.ExecuteScalarAsync<int>(
            query,
            new { MessageProvider = provider.ToString() });
    }
}