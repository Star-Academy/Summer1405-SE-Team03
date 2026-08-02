using MyNewProjectName;

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

string postgresConnectionString = "Host=localhost;Port=5000;Database=test;Username=postgres;Password=postgres";
string sqlServerConnectionString = "Server=localhost,5500;Database=test;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;";

IDatabaseStrategy pgStrategy = new PostgresStrategy();
var pgRunner = new DatabaseRunner(pgStrategy);
pgRunner.Run(postgresConnectionString, postgresResult.sql, query);

IDatabaseStrategy sqlStrategy = new SqlServerStrategy();
var sqlRunner = new DatabaseRunner(sqlStrategy);
sqlRunner.Run(sqlServerConnectionString, sqlserverResult.sql, query);