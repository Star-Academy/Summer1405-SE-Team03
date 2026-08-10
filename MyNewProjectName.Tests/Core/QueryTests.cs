using FluentAssertions;
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
        // Arrange
        // Act
        // Assert
        _sut.TableName.Should().BeEmpty();
        _sut.Columns.Should().BeEmpty();
        _sut.WhereConditions.Should().BeEmpty();
    }
    
    [Fact]
    public void From_ShouldSetTableNameAndReturnSelf_WhenTableNameIsProvided()
    {
        // Arrange
        var tableName = "student";

        // Act
        var result = _sut.From(tableName);

        // Assert
        _sut.TableName.Should().Be(tableName);
        result.Should().BeSameAs(_sut);
    }

    [Fact]
    public void Select_ShouldAppendColumnsAndReturnSelf_WhenColumnsAreProvided()
    {
        // Arrange
        var expectedColumns = new[] { "studentnumber", "firstname", "lastname" };

        // Act
        var result = _sut.Select("studentnumber").Select("firstname", "lastname");

        // Assert
        _sut.Columns.Should().Equal(expectedColumns);
        result.Should().BeSameAs(_sut);
    }

    [Fact]
    public void Where_ShouldAddConditionsAndReturnSelf_WhenConditionsAreProvided()
    {
        // Arrange
        var query = new Query();
        var expectedConditions = new[]
        {
            new WhereCondition("ismale", false, "="),
            new WhereCondition("grade", 19.24m, "="),
            new WhereCondition("age", 18, ">")
        };

        // Act
        var result = query
            .Where("ismale", false)
            .Where("grade", 19.24m)
            .Where("age", 18, ">");

        // Assert
        query.WhereConditions.Should().Equal(expectedConditions);
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void Query_ShouldSupportMethodChaining_WhenMultipleMethodsAreChained()
    {
        // Arrange
        var query = new Query();
        var expectedColumns = new[] { "studentnumber", "firstname" };

        // Act
        var result = query
            .From("student")
            .Select("studentnumber", "firstname")
            .Where("ismale", false)
            .Where("grade", 19.24m);

        // Assert
        result.Should().BeSameAs(query);
        query.TableName.Should().Be("student");
        query.Columns.Should().Equal(expectedColumns);
        query.WhereConditions.Should().HaveCount(2);
    }
}