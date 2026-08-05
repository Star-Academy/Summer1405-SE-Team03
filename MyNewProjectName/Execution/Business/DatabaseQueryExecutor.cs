using System.Data;
using System.Data.Common;
using MyNewProjectName.Compilers;
using MyNewProjectName.Core;
using MyNewProjectName.Presentation;

namespace MyNewProjectName.Execution
{
    public class DatabaseQueryExecutor(
        IDbProvider dbProvider,
        ISqlCompiler compiler,
        IQueryResultPresenter queryResultPresenter,
        DatabaseOptions options)
        : IQueryExecutor
    {
        public void ExecuteQuery(Query query)
        {
            try
            {
                var compiledResult = compiler.Compile(query);

                using var connection = dbProvider.ConnectionFactory.CreateConnection(options.ConnectionString);
                connection.Open();

                using var command = dbProvider.CommandFactory.CreateCommand(compiledResult.Sql, connection);
                dbProvider.ParameterBinder.AddParameters(command, query);

                using var reader = command.ExecuteReader();
                queryResultPresenter.PresentResults(reader);
            }
            catch (DbException dbEx)
            {
                Console.WriteLine($"[{options.DatabaseName} Storage Error]: {dbEx.Message} (Code: {dbEx.ErrorCode})");
            }
        }
    }
}