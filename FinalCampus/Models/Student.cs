using System.ComponentModel.DataAnnotations;

namespace FinalCampus.Models;

public class Student
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int RegNumber { get; set; }
    public int NationalId { get; set; }
    public int BirthCertNumber { get; set; }
    public decimal FeeBalance { get; set; }
    public string Gender { get; set; }
    public int ClassId { get; set; }
    public int IsDeleted { get; set; } = 0;
}