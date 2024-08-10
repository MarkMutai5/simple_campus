using System.ComponentModel.DataAnnotations;

namespace FinalCampus.Models;

public class Faculty
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Dean { get; set; }
}