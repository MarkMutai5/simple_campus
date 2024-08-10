using System.ComponentModel.DataAnnotations;

namespace FinalCampus.ModelDTOs;

public class StudentListDto
{
    public int? Id { get; set; }
    [Required] public string firstName { get; set; }
    public string? lastName { get; set; }
    [Required] public int regNumber { get; set; }
    [Required] public int nationalId { get; set; }
    [Required] public int birthCertNumber { get; set; }
    [Required] public decimal feeBalance { get; set; }
    [Required] public string gender { get; set; }
    [Required] public int classId { get; set; }
}