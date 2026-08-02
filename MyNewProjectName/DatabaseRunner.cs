using System;
using System.Data;

namespace MyNewProjectName;

public class DatabaseRunner
{
    private readonly IDatabaseStrategy _strategy;

    public DatabaseRunner(IDatabaseStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Run(string connectionString, string sqlText, Query query)
    {
        try
        {
            using IDbConnection conn = _strategy.CreateConnection(connectionString);
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