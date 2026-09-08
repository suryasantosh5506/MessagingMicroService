using Dapper;
using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;
using MessagingMicroservice.Infrastructure.Data;

namespace MessagingMicroservice.Infrastructure.Repositories;

public class UserRepository:IUserRepository
{

    private readonly DapperContext _dapperContext;

    public UserRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    public async Task<bool> ExistsByEmail(string email)
    {
        var query = """
                      SELECT ID FROM [dbo].[tblUsers]
                      WHERE [Email] = @email
                    """;
        using var connection=_dapperContext.GetConnection();
        var id=await connection.ExecuteScalarAsync<int?>(query,new {email});
        return id is not null;
    }

    public async Task<int> InsertUser(UserModel user)
    {
        var query = """
                        INSERT INTO [dbo].[tblUsers](
                            [Email],
                            [PasswordHash]
                        )
                        OUTPUT INSERTED.ID
                        VALUES(
                            @email,
                            @passwordHash
                        );
                    """;
        using var connection = _dapperContext.GetConnection();
        return await connection.ExecuteScalarAsync<int>(query,new {email=user.Email,passwordHash=user.PasswordHash});
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        var query = """
                      SELECT * FROM [dbo].[tblUsers]
                      WHERE [Email] = @email
                    """;
        using var connection=_dapperContext.GetConnection();
        var user = await connection.QueryFirstOrDefaultAsync<UserEntity?>(query,new {email});
        return user;
    }
}