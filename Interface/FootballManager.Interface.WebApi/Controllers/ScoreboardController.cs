using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;

[ApiController]
[Route("[controller]"), Authorize]
public class ScoreboardController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<ScoreboardController> logger;

    public ScoreboardController(ILogger<ScoreboardController> logger, IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }
    /// <summary>
    /// Get Scoreboard
    /// </summary>
    /// <returns></returns>
    [HttpGet("leaderboard"), AllowAnonymous]
    public async Task<ActionResult> GetLeaderBoard()
    {
        var result = await unitOfWork.ScoreboardRepository.GetLeaderBoardAsync();
        return Ok(result);
    }
    /// <summary>
    /// Get Leaderboard
    /// </summary>
    /// <returns></returns>
    [HttpGet("winner"), AllowAnonymous]
    public async Task<ActionResult> GetWinner([FromQuery] int teamSize)
    {
        var result = await unitOfWork.ScoreboardRepository.GetWinnerAsync(teamSize);
        return Ok(result);
    }
}
