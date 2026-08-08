using System;
using System.Data;
using Microsoft.Data.SqlClient;
using MyNewProjectName.Execution.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class SqlServerConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}