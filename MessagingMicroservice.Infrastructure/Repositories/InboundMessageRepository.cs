using Dapper;
using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Infrastructure.Data;

namespace MessagingMicroservice.Infrastructure.Repositories;

public class InboundMessageRepository:IInboundMessageRepository
{
    
    private readonly DapperContext _dapperContext;

    public InboundMessageRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    public async Task<InboundMessageEntity?> GetMessage(int id)
    {
        using var connection = _dapperContext.GetConnection();
        var query = """
                        select * from dbo.tblInboundMessages where id = @id;
                    """;
        var result=await connection.QueryFirstOrDefaultAsync<InboundMessageEntity>(query,new{id});
        return result;
    }

    public async Task<int> InsertMessage(InboundMessageEntity message)
    {
        var query = """
                        INSERT INTO dbo.tblInboundMessages (
                            ProviderMessageId,
                            FromNumber,
                            ToNumber,
                            Content,
                            ProviderId,
                            CreatedAt
                        )
                        OUTPUT INSERTED.Id
                        VALUES(
                            @ProviderMessageId,
                            @FromNumber,
                            @ToNumber,
                            @Content,
                            @ProviderId,
                            @CreatedAt
                        )
                    """;
        
        using var connection = _dapperContext.GetConnection();
        int id=await connection.ExecuteScalarAsync<int>(query, message);
        return id;
    }
}