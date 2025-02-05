using Backend.Api.ViewModels;
using Login.DTO;
using Login.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly LoginService _loginService;

    public LoginController(ILogger<LoginController> logger, LoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody loginRequestBody)
    {
        _logger.LogInformation("Login endpoint called");
        try
        {
            var result = await _loginService.Login(loginRequestBody.Username, loginRequestBody.Password);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Unauthorized();
        }
    }
}