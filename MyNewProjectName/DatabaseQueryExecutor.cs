using System;
using System.Data;
using System.Data.Common;

namespace MyNewProjectName
{
    public class DatabaseQueryExecutor : QueryExecutor
    {
        private readonly IDatabaseStrategy _strategy;
        private readonly ISqlCompiler _compiler;
        private readonly string _connectionString;
        private readonly QueryResultPresenter _queryResultPresenter;

        public DatabaseQueryExecutor(
            IDatabaseStrategy strategy,
            ISqlCompiler compiler,
            string connectionString,
            QueryResultPresenter queryResultPresenter)
        {
            _strategy = strategy;
            _compiler = compiler;
            _connectionString = connectionString;
            _queryResultPresenter = queryResultPresenter;
        }

        public void ExecuteQuery(Query query)
        {
            try
            {
                var compiledResult = _compiler.Compile(query);

                using IDbConnection connection = _strategy.CreateConnection(_connectionString);
                connection.Open();

                using IDbCommand command = _strategy.CreateCommand(compiledResult.Sql, connection);
                _strategy.AddParameters(command, query);

                using IDataReader reader = command.ExecuteReader();

                _queryResultPresenter.PresentResults(reader);
            }
            catch (DbException dbEx)
            {
                Console.WriteLine($"[{_strategy.DatabaseName} Storage Error]: {dbEx.Message} (Code: {dbEx.ErrorCode})");
            }
        }
    }
}