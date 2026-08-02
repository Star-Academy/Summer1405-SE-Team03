using MyNewProjectName;
using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;

var query = new Query()
    .From("student")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", 19.24m);

ISqlDialect postgresDialect = new PostgresDialect();
ISqlDialect sqlServerDialect = new SqlServerDialect();

ISqlCompiler postgresCompiler = new SqlCompiler(postgresDialect);
ISqlCompiler sqlServerCompiler = new SqlCompiler(sqlServerDialect);

var postgresResult = postgresCompiler.Compile(query);
var sqlserverResult = sqlServerCompiler.Compile(query);

Console.WriteLine(postgresResult.sql);
Console.WriteLine(postgresResult.binding);

Console.WriteLine(sqlserverResult.sql);
Console.WriteLine(sqlserverResult.binding);


string postgresConnectionString = "Host=localhost;Port=5000;Database=test;Username=postgres;Password=postgres";
string sqlServerConnectionString = "Server=localhost,5500;Database=test;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;";

RunPostgres(postgresConnectionString, postgresResult.sql, query);
RunSqlServer(sqlServerConnectionString, sqlserverResult.sql, query);


static void RunPostgres(string connection, string text, Query query)
{
    NpgsqlConnection conn = new NpgsqlConnection(connection);
    NpgsqlCommand? cmd = null;
    NpgsqlDataReader? reader = null;
    try
    {
        conn.Open();

        using (var schemaCmd = new NpgsqlCommand("SET search_path TO \"TEST-SH\";", conn))
        {
            schemaCmd.ExecuteNonQuery();
        }

        cmd = new NpgsqlCommand(text, conn);
        foreach (var param in query.ColumnNameValue)
        {
            cmd.Parameters.Add(new NpgsqlParameter { Value = param.Value });
        }

        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"Student Number: {reader["studentnumber"]}, Name: {reader["firstname"]}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"PostgreSQL Error: {ex.Message}");
    }
    finally
    {
        if (reader != null && !reader.IsClosed) reader.Close();
        if (cmd != null) cmd.Dispose();
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}

static void RunSqlServer(string connection, string text, Query query)
{
    SqlConnection conn = new SqlConnection(connection);
    SqlCommand? cmd = null;
    SqlDataReader? reader = null;
    int sqlIndex = 0;
    try
    {
        conn.Open();
        cmd = new SqlCommand(text, conn);
        foreach (var param in query.ColumnNameValue)
        {
            cmd.Parameters.AddWithValue($"@p{sqlIndex}", param.Value);
            sqlIndex++;
        }

        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"StudentNumber: {reader["studentnumber"]}, FirstName: {reader["firstname"]}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SQLSERVER Error: {ex.Message}");
    }
    finally
    {
        if (reader != null && !reader.IsClosed) reader.Close();
        if (cmd != null) cmd.Dispose();
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}