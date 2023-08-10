using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;
/// <summary>
/// Match Detail Controller
/// </summary>
[ApiController]
[Route("[controller]")]
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
    /// Get all members
    /// </summary>
    /// <returns>List member</returns>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var members = await unitOfWork.MatchDetailRepository.GetAsync();
        return Ok(members);
    }
    
    /// <summary>
    /// Get one member
    /// </summary>
    /// <param name="id">MatchDetail id</param>
    /// <returns>A member</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await unitOfWork.MatchDetailRepository.GetAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Add a member
    /// </summary>
    /// <param name="member">MatchDetail model</param>
    /// <returns>MatchDetail info</returns>
    [HttpPost]
    public async Task<ActionResult> Add(MatchDetail member)
    {
        var result = await unitOfWork.MatchDetailRepository.AddAsync(member);
        return Ok(result);
    }
    /// <summary>
    /// Update a member information
    /// </summary>
    /// <param name="member">member model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPut]
    public async Task<ActionResult> Update(MatchDetail member)
    {
        var result = await unitOfWork.MatchDetailRepository.UpdateAsync(member);
        return Ok(result);
    }
    /// <summary>
    /// Delete a member
    /// </summary>
    /// <param name="id">member id</param>
    /// <returns>true: deleted, false: error</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await unitOfWork.MatchDetailRepository.DeleteAsync(id);
        return Ok(result);
    }
}
