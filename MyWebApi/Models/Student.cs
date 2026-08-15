namespace MyWebApi.Models;

public class Student
{
    public int StudentNumber { get; set; }
    public string? FirstName { get; set; }
    public bool IsMale { get; set; }
    public decimal Grade { get; set; }
}