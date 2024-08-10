using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinalCampus.Context;
using FinalCampus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinalCampus.Repositories.Auth;

public interface ITokenService
{
    Task<string> GenerateRefreshToken(string userId, DateTime expiry);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

public class TokenService : ITokenService
{
    private readonly ApplicationContext _dbContext;

    public TokenService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    } 
    
    
    public async Task<string> GenerateRefreshToken(string userId, DateTime expiry)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var result =  Convert.ToBase64String(randomNumber);

        var refreshTokeBody = new RefreshTokens
        {
            UserId = userId,
            RefreshToken = result,
            Expiry = expiry,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
        };
        
        var priorRefreshTokens = await _dbContext.RefreshTokens
            .Where(a => a.UserId == userId)
            .ToListAsync();

        foreach (var token in priorRefreshTokens)
        {
            token.IsDeleted = true;
        }

        await _dbContext.RefreshTokens.AddAsync(refreshTokeBody);
        await _dbContext.SaveChangesAsync();

        return result;
    }
}