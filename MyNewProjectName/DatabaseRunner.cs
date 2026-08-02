using System;
using System.Data;
namespace MyNewProjectName;

public class DatabaseRunner
{
    private readonly IDatabaseStrategy _strategy;
    private readonly ISqlCompiler _compiler;
    private readonly string _connectionString;

    public DatabaseRunner(IDatabaseStrategy strategy, ISqlCompiler compiler, string connectionString)
    {
        _strategy = strategy;
        _compiler = compiler;
        _connectionString = connectionString;
    }

    public void Run(Query query)
    {
        try
        {
            var compiledResult = _compiler.Compile(query);
            string sqlText = compiledResult.sql;

            using IDbConnection conn = _strategy.CreateConnection(_connectionString);
            conn.Open();

            _strategy.PreExecuteSetup(conn);

            using IDbCommand cmd = _strategy.CreateCommand(sqlText, conn);
            _strategy.AddParameters(cmd, query);

            using IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Student Number: {reader["studentnumber"]}, Name: {reader["firstname"]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{_strategy.DatabaseName} Error: {ex.Message}");
        }
    }
}