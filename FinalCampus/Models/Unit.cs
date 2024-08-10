using System.ComponentModel.DataAnnotations;

namespace FinalCampus.Models;

public class Unit
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string LecName { get; set; }
}