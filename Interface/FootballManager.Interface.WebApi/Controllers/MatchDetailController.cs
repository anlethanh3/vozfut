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
[Route("[controller]"), Authorize]
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
    /// <summary>
    /// Get members of match
    /// </summary>
    /// <param name="id">MatchDetail id</param>
    /// <returns>list members of match</returns>
    [HttpGet("{id}/members")]
    public async Task<ActionResult> GetMembers(int id)
    {
        var result = await unitOfWork.MatchDetailRepository.GetMembersAsync(id);
        return Ok(result);
    }
}
