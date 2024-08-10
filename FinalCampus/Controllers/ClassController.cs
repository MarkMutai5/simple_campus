using FinalCampus.Context;
using FinalCampus.ModelDTOs;
using FinalCampus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalCampus.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ClassController : ControllerBase
{
    private readonly ApplicationContext _dbContext;

    public ClassController(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("getClasses")]
    public async Task<IActionResult> GetClasses()
    {
        var classes = await _dbContext.Classes.ToListAsync();
        return Ok(classes);
    }
    
    [HttpPost("addClass")]
    public async Task<IActionResult> AddClass([FromBody] ClassListDto item)
    {
        if (ModelState.IsValid)
        {
            var newClass = new Class
            {
                Name =  item.name,
                FacultyId =  item.facultyId
            };
            await _dbContext.Classes.AddAsync(newClass);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        return new JsonResult("Bad Request") { StatusCode = 400 };
    }
}