using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Backend.Api.Controllers;

[ApiController]
public class MetadataController : ControllerBase
{
    private readonly ILogger _logger;

    public MetadataController(ILogger logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("version")]
    public async Task<IActionResult> GetVersion()
    {
        try
        {
            var informationalVersion = Assembly.GetExecutingAssembly().GetName().Version;
            
            if (informationalVersion is null)
            {
                throw new Exception("Version is null");
            }
            
            return Ok(informationalVersion);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Something went wrong while getting version");
            return BadRequest(e.Message);
        }
    }
}