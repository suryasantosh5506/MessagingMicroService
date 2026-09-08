using Dapper;
using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;
using MessagingMicroservice.Infrastructure.Data;

namespace MessagingMicroservice.Infrastructure.Repositories;

public class OutboundMessageRepository:IOutboundMessageRepository
{
    
    private readonly DapperContext _dapperContext;
    
    public OutboundMessageRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    public async Task<OutboundMessageEntity?> GetMessageById(int id)
    {
        using var connection=_dapperContext.GetConnection();
        string query = """
                        select * from dbo.tblOutboundMessages where Id=@id;
                       """;
        var result=await connection.QueryFirstOrDefaultAsync<OutboundMessageEntity>(query,new {id});
        return result;
    }

    public async Task<OutboundMessageEntity?> GetMessageByProviderId(string providerMessageId)
    {
        using var connection=_dapperContext.GetConnection();
        string query = """
                        select * from dbo.tblOutboundMessages where ProviderMessageId=@providerMessageId;
                       """;
        var result=await connection.QueryFirstOrDefaultAsync<OutboundMessageEntity>(query,new {providerMessageId});
        return result;
    }

    public async Task<int> InsertMessage(OutboundMessageEntity outboundMessage)
    {
        using var connection = _dapperContext.GetConnection();

        string query = """
                        INSERT INTO dbo.tblOutboundMessages
                        (
                            ProviderMessageId,
                            FromNumber,
                            ToNumber,
                            ProviderId,
                            Status,
                            CreatedAt,
                            UpdatedAt
                        )
                        OUTPUT INSERTED.Id
                        VALUES
                        (
                            @ProviderMessageId,
                            @FromNumber,
                            @ToNumber,
                            @ProviderId,
                            @Status,
                            @CreatedAt,
                            @UpdatedAt
                        );
                       """;

        var id = await connection.ExecuteScalarAsync<int>(
            query,
            outboundMessage);

        return id;
    }

    public Task UpdateMessage(UpdateMessageStatus request)
    {
        var query = """
                      UPDATE dbo.tblOutboundMessages
                      set status=@status,UpdatedAt=@updatedAt
                      where ProviderMessageId=@providerMessageId;
                    """;
        
        using var connection=_dapperContext.GetConnection();
        return connection.ExecuteAsync(query,new {status=request.Status.ToString(),updatedAt=DateTime.UtcNow,providerMessageId=request.ProviderMessageId});
    }
}