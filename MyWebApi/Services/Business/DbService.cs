using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;
using MyWebApi.Services.Abstractions;

namespace MyWebApi.Services.Business;

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;

    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public QueryFactory GetQueryFactory(string dbType)
    {
        if (string.IsNullOrWhiteSpace(dbType))
        {
            throw new ArgumentException("پارامتر db مشخص نشده است.");
        }

        return dbType.Trim().ToLower() switch
        {
            "postgres" => new QueryFactory(
                new NpgsqlConnection(_configuration.GetConnectionString("PostgresConnection")),
                new PostgresCompiler(),
                30),

            "sqlserver" => new QueryFactory(
                new SqlConnection(_configuration.GetConnectionString("SqlServerConnection")),
                new SqlServerCompiler(),
                30),

            _ => throw new ArgumentException($"دیتابیس '{dbType}' نامعتبر است. مقادیر مجاز: postgres یا sqlserver")
        };
    }
}