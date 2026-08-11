using SqlKata;
using SqlKata.Compilers;

var query = new Query("Students")
    .Select("studentnumber", "firstname")
    .Where("ismale", false)
    .Where("grade", ">=", 19.24)
    .Where("firstname", "Reza")
    .Join("Courses" , "Students.classId", "Courses.classId" , "=" , "outer join")
    .OrderBy("grade")
    .Limit(100)
    .WhereBetween("grade", 18, 19.99);

var postgresCompiler = new PostgresCompiler();
var postgresResult = postgresCompiler.Compile(query);

var sqlServerCompiler = new SqlServerCompiler();
var sqlServerResult = sqlServerCompiler.Compile(query);

Console.WriteLine(postgresResult);
Console.WriteLine(sqlServerResult);

Console.WriteLine("SQL:");
Console.WriteLine(postgresResult.Sql);


Console.WriteLine("Bindings:");
for (int i = 0; i < postgresResult.Bindings.Count; i++)
{
    var value = postgresResult.Bindings[i];

    Console.WriteLine($"@p{i} = {value}");
}

Console.WriteLine("SQL:");
Console.WriteLine(sqlServerResult.Sql);
Console.WriteLine("Bindings:");
for (int i = 0; i < sqlServerResult.Bindings.Count; i++)
{
    var value = sqlServerResult.Bindings[i];

    Console.WriteLine($"@p{i} = {value}");
}