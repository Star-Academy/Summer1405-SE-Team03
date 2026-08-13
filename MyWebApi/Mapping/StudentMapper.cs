using MyWebApi.Dtos.Student;
using MyWebApi.Models;

namespace MyWebApi.Mapping;

public static class StudentMapper
{
    public static StudentResponse ToResponse(this Student student)
    {
        return new StudentResponse
        {
            StudentNumber = student.StudentNumber,
            FirstName = student.FirstName,
            IsMale = student.IsMale,
            Grade = student.Grade
        };
    }

    public static Student ToEntity(this CreateStudentRequest request)
    {
        return new Student
        {
            StudentNumber = request.StudentNumber,
            FirstName = request.FirstName,
            IsMale = request.IsMale,
            Grade = request.Grade
        };
    }

    public static Student ToEntity(this UpdateStudentRequest request, int studentNumber)
    {
        return new Student
        {
            StudentNumber = studentNumber,
            FirstName = request.FirstName,
            IsMale = request.IsMale,
            Grade = request.Grade
        };
    }
}