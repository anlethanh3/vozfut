using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using FootballManager.Data.Entity.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;
/// <summary>
/// Match Controller
/// </summary>
[ApiController]
[Route("[controller]"), Authorize]
public class MatchController : ControllerBase
{
    private readonly ILogger<MatchController> logger;
    private readonly IUnitOfWork unitOfWork;
    /// <summary>
    /// Match controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public MatchController(ILogger<MatchController> logger, IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }
    /// <summary>
    /// Get all matches
    /// </summary>
    /// <returns>List member</returns>
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult> GetAll()
    {
        var matches = await unitOfWork.MatchRepository.GetAsync();
        return Ok(matches);
    }
    /// <summary>
    /// Get one member
    /// </summary>
    /// <param name="id">Match id</param>
    /// <returns>A member</returns>
    [HttpGet("{id}"), AllowAnonymous]
    public async Task<ActionResult> Get(int id)
    {
        var result = await unitOfWork.MatchRepository.GetAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Add a member
    /// </summary>
    /// <param name="member">Match model</param>
    /// <returns>Match info</returns>
    [HttpPost]
    public async Task<ActionResult> Add(Match member)
    {
        var result = await unitOfWork.MatchRepository.AddAsync(member);
        return Ok(result);
    }
    /// <summary>
    /// Search matches with name
    /// </summary>
    /// <param name="request">Search model</param>
    /// <returns>Paging matches</returns>
    [HttpPost("search"), AllowAnonymous]
    public async Task<ActionResult> Search(SearchPagingRequest request)
    {
        var matches = await unitOfWork.MatchRepository.SearchAsync(request.Name);
        var result = matches.Skip(request.PageIndex * request.PageSize).Take(request.PageSize).OrderByDescending(x => x.ModifiedDate);
        return Ok(new Paging<IEnumerable<Match>>
        {
            Data = result,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalPage = (matches.Count() / request.PageSize) + 1,
        });
    }
    /// <summary>
    /// Update a member information
    /// </summary>
    /// <param name="member">member model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(Match member)
    {
        var result = await unitOfWork.MatchRepository.UpdateAsync(member);
        return Ok(result);
    }
    /// <summary>
    /// Delete a member
    /// </summary>
    /// <param name="id">member id</param>
    /// <returns>true: deleted, false: error</returns>
    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await unitOfWork.MatchRepository.DeleteAsync(id);
        return Ok(result);
    }
}
