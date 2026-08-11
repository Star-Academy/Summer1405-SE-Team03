using System;
using System.Data;
using System.Data.Common;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Presentation.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class DatabaseQueryRunner : ICompiledQueryRunner
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IDbCommandFactory _dbCommandFactory;
    private readonly IQueryParameterBinder _queryParameterBinder;

    public DatabaseQueryRunner(
        IDbConnectionFactory dbConnectionFactory,
        IDbCommandFactory dbCommandFactory,
        IQueryParameterBinder queryParameterBinder,
        IQueryResultPresenter queryResultPresenter)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        _dbCommandFactory = dbCommandFactory ?? throw new ArgumentNullException(nameof(dbCommandFactory));
        _queryParameterBinder = queryParameterBinder ?? throw new ArgumentNullException(nameof(queryParameterBinder));
    }

    public IDataReader QueryRunner(CompiledQuery compiledQuery, Query originalQuery)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            
            using var command = _dbCommandFactory.CreateCommand(compiledQuery.SqlQuery, connection);

            var parameters = _queryParameterBinder.BindParameters(originalQuery);
            foreach (var param in parameters)
            {
                var dbParam = command.CreateParameter();
                dbParam.ParameterName = param.Name;
                dbParam.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(dbParam);
            }

            var reader = command.ExecuteReader();
            return reader;
        }
        catch (DbException dbEx)
        {
            throw new InvalidOperationException($"Database Execution Error: {dbEx.Message}", dbEx);
        }
    }
}