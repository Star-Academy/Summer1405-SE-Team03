using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Dtos.Student;

public class CreateStudentRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Student number must be greater than zero.")]
    public int StudentNumber { get; set; }

    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string? FirstName { get; set; }

    public bool IsMale { get; set; }

    [Range(0, 20, ErrorMessage = "Grade must be between 0 and 20.")]
    public decimal Grade { get; set; }
}