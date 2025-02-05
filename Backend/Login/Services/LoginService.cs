using Login.DTO;
using Microsoft.Extensions.Logging;

namespace Login.Services;

public class LoginService
{
    private readonly ILogger<LoginService> _logger;

    public LoginService(ILogger<LoginService> logger)
    {
        _logger = logger;
    }

    public LoginResponse Login(string username, string password)
    {
        _logger.LogInformation("Login endpoint called");
        
        

        return new LoginResponse();
    }
}