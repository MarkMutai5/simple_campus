using FinalCampus.Context;
using FinalCampus.ModelDTOs;
using FinalCampus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalCampus.Controllers;

[Authorize]
[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    private readonly ApplicationContext _dbContext;

    public StudentController(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("getStudents")]
    public async Task<IActionResult> GetStudents()
    {
        var response = new ResponseObject();

        try
        {
            var students = await _dbContext.Students.Where(s => s.IsDeleted != 1).ToListAsync();
            var studentsDto = students.Select(s => new StudentListDto
            {
                Id = s.Id,
                birthCertNumber = s.BirthCertNumber,
                classId = s.ClassId,
                feeBalance = s.FeeBalance,
                firstName = s.FirstName,
                gender = s.Gender,
                lastName = s.LastName,
                nationalId = s.NationalId,
                regNumber = s.RegNumber
            }).ToList();

            response.StatusCode = 200;
            response.Success = true;
            response.Result = studentsDto;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.StatusCode = 400;
            response.Success = false;
            response.Error = ex.Message;
            return BadRequest();
        }
    }

    [HttpPost("addStudent")]
    public async Task<IActionResult> AddStudent([FromBody] StudentListDto student)
    {
        if (ModelState.IsValid)
        {
            var response = new ResponseObject();
            try
            {
                var newStudent = new Student
                {
                    FirstName = student.firstName,
                    LastName = student.lastName,
                    BirthCertNumber = student.birthCertNumber,
                    ClassId = student.classId,
                    FeeBalance = student.feeBalance,
                    Gender = student.gender,
                    NationalId = student.nationalId,
                    RegNumber = student.regNumber
                };
                await _dbContext.Students.AddAsync(newStudent);
                await _dbContext.SaveChangesAsync();
                response.StatusCode = 201;
                response.Success = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                response.StatusCode = 400;
                response.Success = false;
                response.Error = e.Message;
                return BadRequest();
            }
        }

        return new JsonResult("Bad Request") { StatusCode = 400 };
    }


    [HttpGet("getStudent/{Id}")]
    public async Task<IActionResult> GetStudent([FromRoute] int Id)
    {
        try
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == Id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            var studentDto = new StudentListDto
            {
                Id = student.Id,
                birthCertNumber = student.BirthCertNumber,
                classId = student.ClassId,
                feeBalance = student.FeeBalance,
                firstName = student.FirstName,
                gender = student.Gender,
                lastName = student.LastName,
                nationalId = student.NationalId,
                regNumber = student.RegNumber
            };
            return Ok(studentDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("deleteStudent/{Id}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] int Id)
    {
        try
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == Id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            student.IsDeleted = 1;
            await _dbContext.SaveChangesAsync();

            return Ok("Successfully deleted student");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}