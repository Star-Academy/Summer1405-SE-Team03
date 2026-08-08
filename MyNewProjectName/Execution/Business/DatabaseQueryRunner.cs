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
    private readonly IQueryParameterBinder _queryParameterBinder;
    private readonly IQueryResultPresenter _queryResultPresenter;

    public DatabaseQueryRunner(
        IDbConnectionFactory dbConnectionFactory,
        IQueryParameterBinder queryParameterBinder,
        IQueryResultPresenter queryResultPresenter)
    {
        _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        _queryParameterBinder = queryParameterBinder ?? throw new ArgumentNullException(nameof(queryParameterBinder));
        _queryResultPresenter = queryResultPresenter ?? throw new ArgumentNullException(nameof(queryResultPresenter));
    }

    public void QueryRunner(CompiledQuery compiledQuery, Query originalQuery)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = compiledQuery.SqlQuery;

            var parameters = _queryParameterBinder.BindParameters(originalQuery);
            foreach (var param in parameters)
            {
                var dbParam = command.CreateParameter();
                dbParam.ParameterName = param.Name;
                dbParam.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(dbParam);
            }

            using var reader = command.ExecuteReader();
            _queryResultPresenter.PresentResults(reader);
        }
        catch (DbException dbEx)
        {
            throw new InvalidOperationException($"Database Execution Error: {dbEx.Message}", dbEx);
        }
    }
}