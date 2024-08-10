using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinalCampus.Context;
using FinalCampus.ModelDTOs;
using FinalCampus.Models;
using FinalCampus.Repositories.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinalCampus.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class Auth : ControllerBase
{
    private readonly ApplicationContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public Auth(ApplicationContext dbContext, UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    [HttpPost("Signup")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserModel model)
    {
        var response = new ResponseObject();
        try
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                response.StatusCode = 400;
                response.Success = false;
                response.Error = "User exists!";
                return BadRequest(response);
            }

            var user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);


            if (result.Succeeded)
            {
                response.StatusCode = 201;
                response.Success = true;
                response.Result = "User Created Successfully";
                return CreatedAtAction(nameof(RegisterUser), response);
            }
            else
            {
                response.Success = false;
                response.StatusCode = 400;
                response.Error = "Something happened. Try again later.";
                return BadRequest(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response.StatusCode = 400;
            response.Success = false;
            response.Error = "Something happened. Try again later.";
            return BadRequest(response);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(string username, string password)
    {
        var response = new ResponseObject();

        try
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                {
                    var useRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.UserName),
                        new(ClaimTypes.GivenName, $"{user.FirstName}{user.LastName}"),
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach (var userRole in useRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        expires: DateTime.Now.AddMonths(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    var result = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiry = token.ValidTo,
                        UserId = user.Id
                    };
                    response.StatusCode = 200;
                    response.Success = true;
                    response.Result = result;
                    return BadRequest(response);
                }

                response.Success = false;
                response.Error = "Wrong credentials.";
                response.StatusCode = 401;
                return Unauthorized(response);
            }
            else
            {
                response.StatusCode = 400;
                response.Success = false;
                response.Error = "Something happened. Try again later.";
                return BadRequest(response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response.StatusCode = 400;
            response.Success = false;
            response.Error = "Something happened. Try again later.";
            return BadRequest(response);
        }
    }
}