using System.ComponentModel.DataAnnotations;

namespace FinalCampus.ModelDTOs;

public class LecturerListDto
{
    [Required]
    public string firstName { get; set; }
    [Required]
    public string lastName { get; set; }
    [Required]
    public int facultyId { get; set; }
}