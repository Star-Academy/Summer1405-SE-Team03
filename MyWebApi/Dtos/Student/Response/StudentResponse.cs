namespace MyWebApi.Dtos.Student.Response;

public class StudentResponse
{
    public int StudentNumber { get; set; }

    public string? FirstName { get; set; }

    public bool IsMale { get; set; }

    public decimal Grade { get; set; }
}