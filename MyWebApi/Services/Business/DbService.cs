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
    public QueryFactory GetQueryFactory(string dbType)
    {
        switch (dbType)
        {
            case "postgres":
            {
                var npgsqlConnection = new NpgsqlConnection(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION"));
                var queryFactory = new QueryFactory(npgsqlConnection, new PostgresCompiler(), 30);
                return queryFactory;
            }
            case "sqlserver":
            {
                var sqlserverConnection = new SqlConnection(Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION"));
                var queryFactory = new QueryFactory(sqlserverConnection, new SqlServerCompiler(), 30);
                return queryFactory;
            }
            default:
                throw new Exception("DB Type not set");
        }
    }
}