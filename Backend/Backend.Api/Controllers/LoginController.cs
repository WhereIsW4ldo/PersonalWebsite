using Backend.Api.ViewModels;
using Login.Exceptions;
using Login.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Backend.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly Serilog.ILogger _logger;
    private readonly LoginService _loginService;

    public LoginController(Serilog.ILogger logger, LoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequestBody loginRequestBody)
    {
        using (LogContext.PushProperty("Endpoint", "Register"))
        {
            _logger.Information("Register endpoint called");
        
            try
            {
                var result = await _loginService.Register(loginRequestBody.Username, loginRequestBody.Password);
                return Ok(result);
            }
            catch (UserAlreadyExistsException)
            { 
                const string message = "User already exists";
                _logger.Information(message);
                return Conflict(message);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Something went wrong while registering {username}: {stacktrace}", loginRequestBody.Username, e.StackTrace);
                return BadRequest();
            }
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody loginRequestBody)
    {
        using (LogContext.PushProperty("Endpoint", "Login"))
        {
            _logger.Information("Login endpoint called in controller");

            try
            {
                var result = await _loginService.Login(loginRequestBody.Username, loginRequestBody.Password);
                return Ok(result);
            }
            catch (UnauthorizedException)
            {
                _logger.Warning("User {username} is not authenticated", loginRequestBody.Username);
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Something went wrong while logging in {username}", loginRequestBody.Username);
                return BadRequest();
            }
        }
    }
}