using Microsoft.Extensions.Logging;

namespace Login.Services;

public class TokenService
{
    private readonly ILogger<TokenService> _logger;

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public (string accessToken, string refreshToken) GenerateTokens(string userName)
    {
        return (userName, userName);
    }
}