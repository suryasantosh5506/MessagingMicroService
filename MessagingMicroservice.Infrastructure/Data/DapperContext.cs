using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MessagingMicroservice.Infrastructure.Data;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration["ConnectionStrings:DefaultConnection"]??throw new InvalidOperationException("Connection string not found.");;
    }
    
    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}