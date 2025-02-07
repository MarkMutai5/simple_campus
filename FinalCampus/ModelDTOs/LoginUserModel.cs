using System.ComponentModel.DataAnnotations;

namespace FinalCampus.ModelDTOs;

public class LoginUserModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}