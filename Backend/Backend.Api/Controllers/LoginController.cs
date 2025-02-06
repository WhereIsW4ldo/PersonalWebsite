using Backend.Api.ViewModels;
using Login.DTO;
using Login.Exceptions;
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
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequestBody loginRequestBody)
    {
        _logger.LogInformation("Register endpoint called");
        
        try
        {
            var result = await _loginService.Register(loginRequestBody.Username, loginRequestBody.Password);
            return Ok(result);
        }
        catch (UserAlreadyExistsException)
        { 
            const string message = "User already exists";
            _logger.LogWarning(message);
            return Conflict(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
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
        catch (UnauthorizedException)
        {
            _logger.LogWarning("User {username} is not authenticated", loginRequestBody.Username);
            return Unauthorized();
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong while logging in {username}: {stacktrace}", loginRequestBody.Username, e.StackTrace);
            return BadRequest();
        }
    }
}