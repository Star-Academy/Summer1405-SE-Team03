using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services.Abstractions;
using SqlKata.Execution;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
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
            var students = queryFactory.Query("student").Get<Student>();
            return Ok(students);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id, [FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var student = queryFactory.Query("student")
                .Where("studentnumber", id)
                .FirstOrDefault<Student>();

            if (student == null)
            {
                return NotFound(new { message = $"دانشجویی با شماره {id} پیدا نشد." });
            }

            return Ok(student);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
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
                return StatusCode(500, new { message = "امکان افزودن رکورد وجود ندارد." });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = student.StudentNumber, db },
                student);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
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
                return NotFound(new { message = $"دانشجویی با شماره {id} پیدا نشد." });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id, [FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var deletedRows = queryFactory.Query("student")
                .Where("studentnumber", id)
                .Delete();

            if (deletedRows == 0)
            {
                return NotFound(new { message = $"دانشجویی با شماره {id} پیدا نشد." });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}