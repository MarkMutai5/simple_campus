using System.ComponentModel.DataAnnotations;

namespace FinalCampus.Models;

public class Lecturer
{
    [Key] public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int FacultyId { get; set; }
    // public bool Active { get; set; } = true;
}