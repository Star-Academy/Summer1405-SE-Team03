using MyNewProjectName;
using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;


var query = new Query()
    .From("student")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", 19.24);
PostgresCompiler postgres = new PostgresCompiler();
SqlServerCompiler sqlServer = new SqlServerCompiler();
var postgresResult = postgres.Compile(query);
var sqlserverResult = sqlServer.Compile(query);
string postgresConnectionString = "Host=localhost;Port=5432;Database=mohaymen;Username=postgres;Password=postgres";
string sqlServerConnectionString = "Server=localhost,5500;Database=mohaymen;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;";
RunPostgres(postgresConnectionString, postgres.Compile(query).sql, query);
RunSqlServer(sqlServerConnectionString, sqlServer.Compile(query).sql, query);

static void RunPostgres(string connection, string text, Query query)
{
    NpgsqlConnection conn = new NpgsqlConnection(connection);
    NpgsqlCommand? cmd = null;
    NpgsqlDataReader? reader = null;
    int postgresIndex = 1;
    try
    {
        conn.Open();
        cmd = new NpgsqlCommand(text, conn);
        foreach (var param in query._columnName_value)
        {
            cmd.Parameters.AddWithValue(param.Value); 
        }

        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"StudentNumber: {reader["studentnumber"]}, FirstName: {reader["firstname"]}");
            
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"PostgreSQL Error: {ex.Message}");
    }
    finally
    {
        if (reader != null && !reader.IsClosed)
        {
            reader.Close();
        }

        if (cmd != null)
        {
            cmd.Dispose();
        }

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
        foreach (var param in query._columnName_value)
        {
            cmd.Parameters.AddWithValue($"@p{sqlIndex}", param.Value);
            sqlIndex++;
        }

        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"StudentNumber: {reader["studentnumber"]}, FirstName: {reader["firstname"]}");        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SQLSERVER Error: {ex.Message}");
    }
    finally
    {
        if (reader != null && !reader.IsClosed)
        {
            reader.Close();
        }

        if (cmd != null)
        {
            cmd.Dispose();
        }
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
