using System.ComponentModel.DataAnnotations;
namespace FinalCampus.Models;

public class Class
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public int FacultyId { get; set; }
}