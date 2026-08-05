using System;
using System.Data;
using System.Data.Common;

namespace MyNewProjectName
{
    public class DatabaseQueryExecutor : IQueryExecutor
    {
        private readonly IDbProvider _dbProvider;
        private readonly ISqlCompiler _compiler;
        private readonly QueryResultPresenter _queryResultPresenter;
        private readonly DatabaseOptions _databaseoptions;

        public DatabaseQueryExecutor(
            IDbProvider dbProvider,
            ISqlCompiler compiler,
            QueryResultPresenter queryResultPresenter,
            DatabaseOptions options)
        {
            _dbProvider = dbProvider;
            _compiler = compiler;
            _queryResultPresenter = queryResultPresenter;
            _databaseoptions = options;
        }

        public void ExecuteQuery(Query query)
        {
            try
            {
                var compiledResult = _compiler.Compile(query);

                using IDbConnection connection = _dbProvider.ConnectionFactory.CreateConnection(_databaseoptions.ConnectionString);
                connection.Open();

                using IDbCommand command = _dbProvider.CommandFactory.CreateCommand(compiledResult.Sql, connection);
                _dbProvider.ParameterBinder.AddParameters(command, query);

                using IDataReader reader = command.ExecuteReader();
                _queryResultPresenter.PresentResults(reader);
            }
            catch (DbException dbEx)
            {
                Console.WriteLine($"[{_databaseoptions.DatabaseName} Storage Error]: {dbEx.Message} (Code: {dbEx.ErrorCode})");
            }
        }
    }
}