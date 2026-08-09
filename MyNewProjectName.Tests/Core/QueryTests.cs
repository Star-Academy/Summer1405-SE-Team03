using MyNewProjectName.Core;
using Xunit;

namespace MyNewProjectName.Tests.Core;
public class QueryTests 
{
    private readonly Query _sut;
    public QueryTests()
    {
        _sut = new Query();
    }
    
    [Fact]
    public void Constructor_ShouldHaveDefaultValuesWhenInitialized()
    {
        Assert.Equal(string.Empty, _sut.TableName);
        Assert.Empty(_sut.Columns);
        Assert.Empty(_sut.WhereConditions);
    }
    
    [Fact]
    public void From_ShouldSetTableNameAndReturnSelf_WhenTableNameIsProvided()
    {
        var result = _sut.From("student");

        Assert.Equal("student", _sut.TableName);
        Assert.Same(_sut, result);
    }

    [Fact]
    public void Select_ShouldAppendColumnsAndReturnSelf_WhenColumnsAreProvided()
    {
        var result = _sut.Select("studentnumber").Select("firstname", "lastname");

        Assert.Equal(new[] { "studentnumber", "firstname", "lastname" }, _sut.Columns);
        Assert.Same(_sut, result);
    }

    [Fact]
    public void Where_ShouldAddConditionsAndReturnSelf_WhenConditionsAreProvided()
    {
        var query = new Query();

        var result = query
            .Where("ismale", false)
            .Where("grade", 19.24m)
            .Where("age", 18, ">");

        var expectedConditions = new[]
        {
            new WhereCondition("ismale", false, "="),
            new WhereCondition("grade", 19.24m, "="),
            new WhereCondition("age", 18, ">")
        };

        Assert.Equal(expectedConditions, query.WhereConditions);
        Assert.Same(query, result);
    }

    [Fact]
    public void Query_ShouldSupportMethodChaining_WhenMultipleMethodsAreChained()
    {
        var query = new Query();

        var result = query
            .From("student")
            .Select("studentnumber", "firstname")
            .Where("ismale", false)
            .Where("grade", 19.24m);

        Assert.Same(query, result);
        Assert.Equal("student", query.TableName);
        Assert.Equal(new[] { "studentnumber", "firstname" }, query.Columns);
        Assert.Equal(2, query.WhereConditions.Count);
    }
}