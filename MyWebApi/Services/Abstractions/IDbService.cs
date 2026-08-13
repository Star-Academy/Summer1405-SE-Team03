using SqlKata.Execution;
namespace MyWebApi.Services.Abstractions;

public interface IDbService
{
    QueryFactory GetQueryFactory(string dbType);
}