using System;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Grammars.Abstractions;
using MyNewProjectName.Grammars.Business;
using MyNewProjectName.Presentation.Abstractions;
using MyNewProjectName.Presentation.Business;
using MyNewProjectName.Presentation.Abstractions;
using MyNewProjectName.Presentation.Business;

var query = new Query()
    .From("student")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", 19.24m);

IQueryResultPresenter presenter = new StudentQueryResultPresenter(new DefaultRowFormatter());

var pgConnection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION") 
                   ?? throw new InvalidOperationException("Environment variable 'POSTGRES_CONNECTION' is not set.");

var pgRunner = new DatabaseQueryRunner(
    new PostgresConnectionFactory(pgConnection),
    new PostgresCommandFactory(),
    new PostgresQueryParameterBinder(new PostgresSyntaxFormatter()),
    presenter
);
var pgOrchestrator = new QueryExecutionOrchestrator(
    CreateCompiler(new PostgresSyntaxFormatter()),
    pgRunner
);

var sqlConnection = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION") 
                    ?? throw new InvalidOperationException("Environment variable 'SQLSERVER_CONNECTION' is not set.");
                       
var sqlRunner = new DatabaseQueryRunner(
    new SqlServerConnectionFactory(sqlConnection),
    new SqlServerCommandFactory(),
    new SqlServerQueryParameterBinder(new SqlServerSyntaxFormatter()),
    presenter
);

var sqlOrchestrator = new QueryExecutionOrchestrator(
    CreateCompiler(new SqlServerSyntaxFormatter()),
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