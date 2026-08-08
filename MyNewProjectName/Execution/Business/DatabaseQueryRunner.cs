using System;
using System.Data.Common;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Presentation.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class DatabaseQueryRunner : ICompiledQueryRunner
{
    private readonly IDbProvider _dbProvider;
    private readonly IQueryResultPresenter _presenter;
    private readonly DatabaseOptions _options;

    public DatabaseQueryRunner(
        IDbProvider dbProvider,
        IQueryResultPresenter presenter,
        DatabaseOptions options)
    {
        _dbProvider = dbProvider ?? throw new ArgumentNullException(nameof(dbProvider));
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public void Run(CompiledQuery compiledQuery, Query originalQuery)
    {
        try
        {
            using var connection = _dbProvider.ConnectionFactory.CreateConnection(_options.ConnectionString);
            connection.Open();

            using var command = _dbProvider.CommandFactory.CreateCommand(compiledQuery.SqlQuery, connection);
            _dbProvider.ParameterBinder.AddParameters(command, originalQuery);

            using var reader = command.ExecuteReader();
            _presenter.PresentResults(reader);
        }
        catch (DbException dbEx)
        {
            throw new InvalidOperationException($"[{_options.DatabaseName} Storage Error]: {dbEx.Message}", dbEx);
        }
    }
}