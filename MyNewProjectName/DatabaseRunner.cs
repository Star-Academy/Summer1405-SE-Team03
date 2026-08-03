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
            
            using IDbConnection connection = _strategy.CreateConnection(_connectionString);
            connection.Open();

            _strategy.PreExecuteSetup(connection);

            using IDbCommand command = _strategy.CreateCommand(compiledResult.Sql, connection);
            _strategy.AddParameters(command, query);

            using IDataReader reader = command.ExecuteReader();
            
            PrintResults(reader);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{_strategy.DatabaseName} Error: {ex.Message}");
        }
    }
    
    private void PrintResults(IDataReader reader)
    {
        while (reader.Read())
        {
            Console.WriteLine($"Student Number: {reader["studentnumber"]}, Name: {reader["firstname"]}");
        }
    }
}