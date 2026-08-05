using System;
using MyNewProjectName;

var query = new Query()
    .From("student")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", 19.24m);

QueryResultPresenter presenter = new StudentQueryResultPresenter();

var pgConnection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION") 
                   ?? throw new InvalidOperationException("Environment variable 'POSTGRES_CONNECTION' is not set.");

var pgRunner = new DatabaseQueryExecutor(
    new PostgresDbProvider(), 
    new SqlCompiler(new PostgresGrammar()), 
    presenter,
    new DatabaseOptions(pgConnection, "PostgreSQL")
);

var sqlConnection = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION") 
                    ?? throw new InvalidOperationException("Environment variable 'SQLSERVER_CONNECTION' is not set.");

var sqlRunner = new DatabaseQueryExecutor(
    new SqlServerDbProvider(), 
    new SqlCompiler(new SqlServerGrammar()), 
    presenter,
    new DatabaseOptions(sqlConnection, "SQL Server")
);

pgRunner.ExecuteQuery(query);
sqlRunner.ExecuteQuery(query);