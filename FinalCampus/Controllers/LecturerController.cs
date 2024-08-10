using FinalCampus.Context;
using FinalCampus.ModelDTOs;
using FinalCampus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalCampus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LecturerController : ControllerBase
{
    private readonly ApplicationContext _dbContext;

    public LecturerController(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("getLecturers")]
    public async Task<IActionResult> GetLecturers()
    {
        var lecturers = await _dbContext.Lecturers.ToListAsync();
        return Ok(lecturers);
    }

    [HttpPost("addLecturer")]
    public async Task<IActionResult> AddLecturer([FromBody] LecturerListDto lecturer)
    {
        if (ModelState.IsValid)
        {
            var newLec = new Lecturer
            {
                FirstName = lecturer.firstName,
                LastName = lecturer.lastName,
                FacultyId = lecturer.facultyId
            };

            await _dbContext.Lecturers.AddAsync(newLec);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        return new JsonResult("Bad Request") { StatusCode = 400 };
    }
    
}