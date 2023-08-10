using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    private readonly ILogger<VersionController> logger;

    public VersionController(ILogger<VersionController> logger)
    {
        this.logger = logger;
    }
    /// <summary>
    /// Get all members
    /// </summary>
    /// <returns>List member</returns>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var assembly = GetType().Assembly.GetName();
        return Ok(new
        {
            FullName = assembly.FullName,
            Version = assembly.Version,
            VersionCompat = assembly.VersionCompatibility,
        });
    }
}
