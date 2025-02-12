using Database;
using Database.Models;
using Login.DTO;
using Login.Exceptions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Login.Services;

public class LoginService
{
    private readonly ILogger _logger;
    private readonly UserContext _userContext;
    private readonly TokenService _tokenService;

    public LoginService(ILogger logger, UserContext userContext, TokenService tokenService)
    {
        _logger = logger;
        _userContext = userContext;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Register(string username, string password)
    {
        _logger.Information("Register endpiont called with {username}");
        
        if (await _userContext.LoginCredentials.AnyAsync(x => x.Username == username))
        {
            throw new UserAlreadyExistsException();
        }
        
        await _userContext.LoginCredentials.AddAsync(new LoginCredential
        {
            Username = username,
            Password = password
        });
        
        await _userContext.SaveChangesAsync();

        _logger.Information("Registered user with username: {username}", username);
        
        var (accessToken, refreshToken) = _tokenService.GenerateTokens(username);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponse> Login(string username, string password)
    {
        _logger.Information("Login endpoint called");

        var credentials = await _userContext
            .LoginCredentials
            .Where(x => x.Username == username && x.Password == password)
            .FirstOrDefaultAsync();

        _logger.Information("Credentials are {0}", credentials is null ? "null" : "not null");

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
