using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services.Abstractions;
using SqlKata.Execution;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController:ControllerBase
{
    private readonly IDbService _dbService;
    
    public StudentsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var selectedStudents = queryFactory.Query("student").Get();
            return Ok(selectedStudents);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id, [FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var selectedStudents = queryFactory.Query("student").Where("studentnumber", id).FirstOrDefault<Student>();
            if (selectedStudents == null)
            {
                return NotFound(new { message = $"Student {id} not found" });
            }
            return Ok(selectedStudents);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Create([FromQuery] string db, [FromBody] Student student)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var affectedRows = queryFactory.Query("student").Insert(new
            {
                studentnumber = student.StudentNumber,
                firstname = student.FirstName, 
                ismale = student.IsMale,
                grade = student.Grade
            });
            if (affectedRows == 0)
            {
                return StatusCode(500 , "cant add record");
            }
            return CreatedAtAction(
                nameof(GetById), 
                new { id = student.StudentNumber, db = db }, student);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromQuery] string db, [FromBody] Student student)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var affectedRows = queryFactory.Query("student")
                .Where("studentnumber", id)
                .Update(new
                {
                    firstname = student.FirstName, 
                    ismale = student.IsMale,
                    grade = student.Grade
                });

            if (affectedRows == 0)
            {
                return NotFound($"Student {id} not found");
            }

            student.StudentNumber = id;
            return Ok(student);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id, [FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var deletedStudents = queryFactory.Query("student").Where("studentnumber", id).Delete();
            if (deletedStudents == 0)
            {
                return NotFound($"Student {id} not found");
            }
            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
