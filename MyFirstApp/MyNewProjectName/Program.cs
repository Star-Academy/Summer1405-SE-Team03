using MyNewProjectName;

var query = new Query()
    .From("Students")
    .Select("Id", "Name")
    .Where("IsMale", true)
    .Where("Age", 20);
PostgresCompiler postgres = new PostgresCompiler();
SqlServerCompiler sqlServer = new SqlServerCompiler();
Console.WriteLine(postgres.Compile(query).sql);
Console.WriteLine(postgres.Compile(query).binding);
Console.WriteLine(sqlServer.Compile(query).sql);
Console.WriteLine(sqlServer.Compile(query).binding);