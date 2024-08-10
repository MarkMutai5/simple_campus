using Microsoft.AspNetCore.Identity;

namespace FinalCampus.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<RefreshTokens> RefreshToken { get; set; }
}