using Serilog;

namespace Login.Services;

public class TokenService
{
    private readonly ILogger _logger;

    public TokenService(ILogger logger)
    {
        _logger = logger;
    }

    public (string accessToken, string refreshToken) GenerateTokens(string userName)
    {
        return (userName, userName);
    }
}
