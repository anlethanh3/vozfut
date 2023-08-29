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
        if (result == null)
        {
            return BadRequest("ERR_DATA_IS_EXIST");
        }
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
    /// Update or Create a match detail information
    /// </summary>
    /// <param name="detail">match detail model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPost("update")]
    public async Task<ActionResult> UpdateNew(MatchDetail detail)
    {
        var result = await unitOfWork.MatchDetailRepository.UpdateNewAsync(detail);
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
        var readyPlayers = details.Where(x => x.IsPaid && !x.IsSkip);
        var elos = new int[numTeams];
        if (readyPlayers.Count() < numTeams * match.TeamSize)
        {
            return BadRequest("ERR_TEAM_SIZE");
        }
        var players = readyPlayers
            .Select(x => new Member
            {
                Id = x.MemberId,
                Elo = x.MemberElo,
                Name = x.MemberName,
            })
            .Take(numTeams * match.TeamSize);
        var orders = players.OrderByDescending(item => item.Elo).ToList();
        var teams = new List<Member>[match.TeamCount];
        var rng = new Random();
        // random members
        while (orders.Count() > 0)
        {
            int teamIndex = 0;
            for (var index = 0; index < elos.Count(); index++)
            {
                if (elos[teamIndex] > elos[index])
                {
                    teamIndex = index;
                }
            }
            var maxElos = orders.Where(x => x.Elo == orders[0].Elo).ToList();
            if (teams[teamIndex] is null)
            {
                teams[teamIndex] = new();
            }
            var randomIndex = rng.Next(-1, maxElos.Count() - 1) + 1;
            var player = maxElos[randomIndex];
            teams[teamIndex].Add(player);
            elos[teamIndex] += player.Elo;
            orders.Remove(player);
        }
        var result = teams.Select(x => new
        {
            Players = x,
            EloSum = x.Sum(m => m.Elo),
        });
        return Ok(result);
    }
}
