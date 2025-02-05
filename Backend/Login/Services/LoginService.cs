using Database;
using Login.DTO;
using Login.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Login.Services;

public class LoginService
{
    private readonly ILogger<LoginService> _logger;
    private readonly UserContext _userContext;
    private readonly TokenService _tokenService;

    public LoginService(ILogger<LoginService> logger, UserContext userContext, TokenService tokenService)
    {
        _logger = logger;
        _userContext = userContext;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Login(string username, string password)
    {
        _logger.LogInformation("Login endpoint called");

        var credentials = await _userContext
            .LoginCredentials
            .Where(x => x.Username == username && x.Password == password)
            .FirstOrDefaultAsync();

        if (credentials is null)
        {
            throw new UnauthorizedException();
        }
        
        var (accessToken, refreshToken) = _tokenService.GenerateTokens(username);
        
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}