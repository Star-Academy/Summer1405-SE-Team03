using System;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Grammars.Abstractions;
using MyNewProjectName.Grammars.Business;
using MyNewProjectName.Presentation.Abstractions;
using MyNewProjectName.Presentation.Business;

var query = new Query()
    .From("student")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", 19.24m);

IQueryResultPresenter presenter = new StudentQueryResultPresenter();

var pgConnection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION") 
                   ?? throw new InvalidOperationException("Environment variable 'POSTGRES_CONNECTION' is not set.");

var pgRunner = new DatabaseQueryRunner(
    new PostgresDbProvider(),
    presenter,
    new DatabaseOptions(pgConnection, "PostgreSQL")
);

var pgOrchestrator = new QueryExecutionOrchestrator(
    CreateCompiler(new PostgresGrammar()),
    pgRunner
);

var sqlConnection = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION") 
                    ?? throw new InvalidOperationException("Environment variable 'SQLSERVER_CONNECTION' is not set.");
                       
var sqlRunner = new DatabaseQueryRunner(
    new SqlServerDbProvider(), 
    presenter,
    new DatabaseOptions(sqlConnection, "SQL Server")
);

var sqlOrchestrator = new QueryExecutionOrchestrator(
    CreateCompiler(new SqlServerGrammar()),
    sqlRunner
);

pgOrchestrator.ExecuteQuery(query);
sqlOrchestrator.ExecuteQuery(query);

return;

static SqlQueryCompiler CreateCompiler(IDatabaseSpecificSyntaxFormatter grammar)
{
    var selectBuilder = new SelectClauseBuilder(grammar);
    var fromBuilder = new FromClauseBuilder(grammar);
    var conditionProcessor = new WhereConditionProcessor(grammar);
    var whereBuilder = new WhereClauseBuilder(conditionProcessor);
    
    return new SqlQueryCompiler(selectBuilder, fromBuilder, whereBuilder);
}