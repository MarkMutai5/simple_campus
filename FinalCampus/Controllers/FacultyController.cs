using FinalCampus.Context;
using FinalCampus.ModelDTOs;
using FinalCampus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalCampus.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FacultyController : ControllerBase
{
    private readonly ApplicationContext _dbContext;

    public FacultyController(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("getFaculties")]
    public async Task<IActionResult> GetFaculties()
    {
        var faculties = await _dbContext.Faculties.ToListAsync();
        return Ok(faculties);
    }

    [HttpPost("addFaculty")]
    public async Task<IActionResult> AddFaculty([FromBody] FacultyListDto faculty)
    {
        if (ModelState.IsValid)
        {
            var newFaculty = new Faculty
            {
                Name = faculty.name,
                Dean = faculty.dean
            };
            await _dbContext.Faculties.AddAsync(newFaculty);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        return new JsonResult("Bad Request") { StatusCode = 400 };
    }
}