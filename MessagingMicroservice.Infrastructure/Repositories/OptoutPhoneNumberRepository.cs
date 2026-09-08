using Dapper;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;
using MessagingMicroservice.Infrastructure.Data;

namespace MessagingMicroservice.Infrastructure.Repositories;

public class OptoutPhoneNumberRepository:IOptoutPhoneNumberRepository
{
    
    private readonly DapperContext _dapperContext;

    public OptoutPhoneNumberRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    public async Task<int> InsertOptoutPhoneNumber(OptoutPhoneNumberModel optOutDetails)
    {
        var query = """
                        INSERT INTO [dbo].[tblOptoutPhoneNumbers]
                        (
                            [PhoneNumber],
                            [OptedOutAt],
                            [InboundMessageId],
                            [ProviderId]
                        )
                        OUTPUT INSERTED.Id
                        VALUES(
                            @PhoneNumber,
                            @OptedOutAt,
                            @InboundMessageId,
                            @ProviderId
                        );
                     """;
        using var connection = _dapperContext.GetConnection();
        var id = await connection.ExecuteScalarAsync<int>(
            query,new
            {
                PhoneNumber=optOutDetails.PhoneNumber,
                OptedOutAt=optOutDetails.OptedOutAt,
                InboundMessageId=optOutDetails.InboundMessageId,
                ProviderId=optOutDetails.ProviderId
            });
        return id;
    }

    public async Task<bool> IsOptedout(string phoneNumber)
    {
        var query = """
                        SELECT Id FROM [dbo].[tblOptoutPhoneNumbers]
                        WHERE PhoneNumber=@PhoneNumber;
                    """;
        using var connection = _dapperContext.GetConnection();
        var id = await connection.QueryFirstOrDefaultAsync<int?>(query, new
        {
            PhoneNumber = phoneNumber
        });
        return id is not null;
    }

    public async Task<int?> GetOptoutIdByPhoneNumber(string phoneNumber)
    {
        var query = """
                        SELECT Id FROM [dbo].[tblOptoutPhoneNumbers]
                        WHERE PhoneNumber=@PhoneNumber;
                    """;
        using var connection = _dapperContext.GetConnection();
        var id = await connection.QueryFirstOrDefaultAsync<int?>(query, new
        {
            PhoneNumber = phoneNumber
        });
        return id;
    }
}