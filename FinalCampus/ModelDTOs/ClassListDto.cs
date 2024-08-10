using System.ComponentModel.DataAnnotations;

namespace FinalCampus.ModelDTOs;

public class ClassListDto
{
    [Required] public string name { get; set; }
    [Required] public int facultyId { get; set; }
}