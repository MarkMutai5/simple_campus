using System.ComponentModel.DataAnnotations;

namespace FinalCampus.ModelDTOs;

public class FacultyListDto
{
    [Required]
    public string name { get; set; }
    [Required]
    public string dean { get; set; }
}