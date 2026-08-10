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
        ArgumentNullException.ThrowIfNull(connectionString);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be empty or whitespace.", nameof(connectionString));
        }
        _connectionString = connectionString;        
    }
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}