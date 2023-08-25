using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
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
        var numTeams = match.TeamCount;
        var players = details
        .Select(x => new Member
        {
            Id = x.MemberId,
            Elo = x.MemberElo,
            Name = x.MemberName,
        })
        .Take(numTeams * match.TeamSize);
        var rnd = new Random();
        var orders = players.OrderByDescending(item => item.Elo).ToList();
        var teams = new List<Member>[match.TeamCount];

        // random members
        for (int i = 0; i < match.TeamSize; i++)
        {
            var random = orders.Take(match.TeamCount).OrderBy(x => rnd.Next()).ToList();
            for (int j = 0; j < match.TeamCount; j++)
            {
                if (teams[j] is null)
                {
                    teams[j] = new();
                }
                teams[j].Add(random[j]);
                orders.Remove(random[j]);
            }
        }
        var result = teams.Select(x => new
        {
            Players = x,
            EloSum = x.Sum(m => m.Elo),
        });
        return Ok(result);
    }
}
