using MyNewProjectName;

var query = new Query()
    .From("student")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", 19.24m);

string pgConnection = "Host=localhost;Port=5000;Database=test;Username=postgres;Password=postgres";
var pgRunner = new DatabaseRunner(
    new PostgresStrategy(), 
    new SqlCompiler(new PostgresDialect()), 
    pgConnection
);

string sqlConnection = "Server=localhost,5500;Database=test;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;";
var sqlRunner = new DatabaseRunner(
    new SqlServerStrategy(), 
    new SqlCompiler(new SqlServerDialect()), 
    sqlConnection
);

pgRunner.Run(query);
sqlRunner.Run(query);