using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;

[ApiController]
[Route("[controller]"), Authorize(Roles = "Admin")]
public class TeamRivalController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<TeamRivalController> logger;

    public TeamRivalController(ILogger<TeamRivalController> logger, IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }
    /// <summary>
    /// Get Team Division Rivals
    /// </summary>
    /// <param name="id">Match Id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> Create(int id)
    {
        try
        {
            IEnumerable<TeamRival> result;
            var match = await unitOfWork.MatchRepository.GetAsync(id);
            if (match is null)
            {
                return BadRequest("ERR_MATCH_NOT_EXIST");
            }
            if (match.HasTeamRival)
            {
                result = await unitOfWork.TeamRivalRepository.GetAsync(id);
                return Ok(result);
            }
            result = await unitOfWork.TeamRivalRepository.CreateAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Get Team Division Rivals
    /// </summary>
    /// <param name="id">Match Id</param>
    /// <returns></returns>
    [HttpGet("{id}/anonymous"), AllowAnonymous]
    public async Task<ActionResult> Get(int id)
    {
        try
        {
            IEnumerable<TeamRival> result;
            var match = await unitOfWork.MatchRepository.GetAsync(id);
            if (match is null)
            {
                return BadRequest("ERR_MATCH_NOT_EXIST");
            }
            if (match.HasTeamRival)
            {
                result = await unitOfWork.TeamRivalRepository.GetAsync(id);
                return Ok(result);
            }
            return BadRequest("ERR_TEAM_RIVALS_NOT_EXIST");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Exchange member of match
    /// </summary>
    /// <param name="model">Exchange model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPost("{id}/exchange")]
    public async Task<ActionResult> ExchangeMembers([FromBody] ExchangeMemberRequest model)
    {
        try
        {
            var result = await unitOfWork.TeamRivalRepository.ExchangeMemberAsync(model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
