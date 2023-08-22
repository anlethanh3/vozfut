using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using FootballManager.Logic.Business.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;
/// <summary>
/// Match Detail Controller
/// </summary>
[ApiController]
[Route("[controller]"), Authorize(Roles = "Admin")]
public class MatchDetailController : ControllerBase
{
    private readonly ILogger<MatchDetailController> logger;
    private readonly IUnitOfWork unitOfWork;
    /// <summary>
    /// Match detail controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public MatchDetailController(ILogger<MatchDetailController> logger, IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }
    /// <summary>
    /// Get all match detail
    /// </summary>
    /// <returns>List match detail</returns>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var detail = await unitOfWork.MatchDetailRepository.GetAsync();
        return Ok(detail);
    }

    /// <summary>
    /// Get all match detail
    /// </summary>
    /// <returns>List match detail</returns>
    [HttpPost("search")]
    public async Task<ActionResult> GetAll(MatchDetailRequest request)
    {
        var details = await unitOfWork.MatchDetailRepository.GetAllAsync(request.MatchId);
        return Ok(details);
    }

    /// <summary>
    /// Get one match detail
    /// </summary>
    /// <param name="id">MatchDetail id</param>
    /// <returns>A match detail</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await unitOfWork.MatchDetailRepository.GetAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Add a match detail
    /// </summary>
    /// <param name="detail">MatchDetail model</param>
    /// <returns>MatchDetail info</returns>
    [HttpPost]
    public async Task<ActionResult> Add(MatchDetail detail)
    {
        var result = await unitOfWork.MatchDetailRepository.AddAsync(detail);
        return Ok(result);
    }
    /// <summary>
    /// Update a match detail information
    /// </summary>
    /// <param name="detail">match detail model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPut]
    public async Task<ActionResult> Update(MatchDetail detail)
    {
        var result = await unitOfWork.MatchDetailRepository.UpdateAsync(detail);
        return Ok(result);
    }
    /// <summary>
    /// Delete a match detail
    /// </summary>
    /// <param name="id">match detail id</param>
    /// <returns>true: deleted, false: error</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await unitOfWork.MatchDetailRepository.DeleteAsync(id);
        return Ok(result);
    }

    [HttpGet("rolling/{id}")]
    public async Task<ActionResult> Rolling(int id)
    {
        var match = await unitOfWork.MatchRepository.GetAsync(id);
        if (match is null)
        {
            return BadRequest("ERR_MATCH_NOT_EXIST");
        }
        var details = await unitOfWork.MatchDetailRepository.GetAllAsync(id);
        var players = details.Select(x => new Player
        {
            Id = x.MemberId,
            Elo = x.MemberElo,
            Name = x.MemberName,
        }).ToList();
        var numTeams = match.TeamCount;

        RollRepository teamAssignment = new(numTeams, players);
        if (teamAssignment.FindBalancedTeamAssignment())
        {
            var assignedTeams = teamAssignment.GetAssignedTeams();
            var teamBibColors = new string[] { "yellow", "red", "blue", "orange" };

            return Ok(new
            {
                Teams = assignedTeams.Select(x => new
                {
                    Players = x,
                    EloSum = x.Sum(m => m.Elo),
                }),
                Colors = teamBibColors.Take(numTeams)
            });
        }
        return BadRequest("No balanced team assignment found.");
    }
}
