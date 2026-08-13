using Microsoft.Data.SqlClient;
using Npgsql;
using SqlKata.Compilers;
namespace MyWebApi.Services.Business;
using MyWebApi.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using SqlKata.Execution;
using MyWebApi.Services.Abstractions;
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
            throw new ArgumentNullException("dbType is null or empty");
        }
        switch (dbType)
        {
            case "postgres":
            {
                var connectionString = _configuration.GetConnectionString("PostgresConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Postgres connection string not set");
                }
                var npgsqlConnection = new NpgsqlConnection(connectionString);
                
                var queryFactory = new QueryFactory(npgsqlConnection, new PostgresCompiler(), 30);
                return queryFactory;
            }
            case "sqlserver":
            {
                var connectionString = _configuration.GetConnectionString("SqlServerConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Postgres connection string not set");
                }
                var sqlserverConnection = new SqlConnection(connectionString);
                var queryFactory = new QueryFactory(sqlserverConnection, new SqlServerCompiler(), 30);
                return queryFactory;
            }
            default:
                throw new ArgumentOutOfRangeException("dbType is not valid");
        }
    }
}