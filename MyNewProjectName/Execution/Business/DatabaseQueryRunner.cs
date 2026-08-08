using System;
using System.Data.Common;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Presentation.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class DatabaseQueryRunner : ICompiledQueryRunner
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IQueryParameterBinder _parameterBinder;
    private readonly IQueryResultPresenter _presenter;

    public DatabaseQueryRunner(
        IDbConnectionFactory connectionFactory,
        IQueryParameterBinder parameterBinder,
        IQueryResultPresenter presenter)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _parameterBinder = parameterBinder ?? throw new ArgumentNullException(nameof(parameterBinder));
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
    }

    public void QueryRunner(CompiledQuery compiledQuery, Query originalQuery)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = compiledQuery.SqlQuery;

            _parameterBinder.AddParameters(command, originalQuery);

            using var reader = command.ExecuteReader();
            _presenter.PresentResults(reader);
        }
        catch (DbException dbEx)
        {
            throw new InvalidOperationException($"Database Execution Error: {dbEx.Message}", dbEx);
        }
    }
}