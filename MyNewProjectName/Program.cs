using System;
using MyNewProjectName;

var query = new Query()
    .From("student")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", 19.24m);

QueryResultPresenter presenter = new StudentQueryResultPresenter();
static SqlCompiler CreateCompiler(ISqlGrammar grammar)
{
    var selectBuilder = new SelectClauseBuilder(grammar);
    var fromBuilder = new FromClauseBuilder(grammar);
    var conditionProcessor = new WhereConditionProcessor(grammar);
    var whereBuilder = new WhereClauseBuilder(conditionProcessor);
    
    return new SqlCompiler(selectBuilder, fromBuilder, whereBuilder);
}

string pgConnection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION") 
                      ?? throw new InvalidOperationException("Environment variable 'POSTGRES_CONNECTION' is not set.");

var pgRunner = new DatabaseQueryExecutor(
    new PostgresStrategy(), 
    CreateCompiler(new PostgresGrammar()),
    pgConnection,
    presenter
);

string sqlConnection = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION") 
                       ?? throw new InvalidOperationException("Environment variable 'SQLSERVER_CONNECTION' is not set.");
                       
var sqlRunner = new DatabaseQueryExecutor(
    new SqlServerStrategy(), 
    CreateCompiler(new SqlServerGrammar()),
    sqlConnection,
    presenter
);

pgRunner.ExecuteQuery(query);
sqlRunner.ExecuteQuery(query);