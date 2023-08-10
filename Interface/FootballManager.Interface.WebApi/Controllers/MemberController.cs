using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using FootballManager.Data.Entity.Results;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;
/// <summary>
/// Member Controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class MemberController : ControllerBase
{
    private readonly ILogger<MemberController> logger;
    private readonly IUnitOfWork unitOfWork;
    /// <summary>
    /// Member controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public MemberController(ILogger<MemberController> logger, IUnitOfWork unitOfWork)
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
        var members = await unitOfWork.MemberRepository.GetAsync();
        return Ok(members);
    }
    /// <summary>
    /// Paging members
    /// </summary>
    /// <param name="paging">Paging model</param>
    /// <returns>Paging members</returns>
    [HttpGet("paging")]
    public async Task<ActionResult> GetPaging([FromQuery]Paging<bool> paging)
    {
        var members = await unitOfWork.MemberRepository.GetAsync();
        var result = members.Skip(paging.PageIndex * paging.PageSize).Take(paging.PageSize);
        return Ok(new Paging<IEnumerable<Member>>
        {
            Data = result,
            PageIndex = paging.PageIndex,
            PageSize = paging.PageSize,
            TotalPage = (members.Count() / paging.PageSize) + 1,
        });
    }
    /// <summary>
    /// Get one member
    /// </summary>
    /// <param name="id">Member id</param>
    /// <returns>A member</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await unitOfWork.MemberRepository.GetAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Add a member
    /// </summary>
    /// <param name="member">Member model</param>
    /// <returns>Member info</returns>
    [HttpPost]
    public async Task<ActionResult> Add(Member member)
    {
        var result = await unitOfWork.MemberRepository.AddAsync(member);
        return Ok(result);
    }
    /// <summary>
    /// Search members with name
    /// </summary>
    /// <param name="request">Search model</param>
    /// <returns>Paging members</returns>
    [HttpPost("search")]
    public async Task<ActionResult> Search(SearchPagingRequest request)
    {
        var members = await unitOfWork.MemberRepository.SearchAsync(request.Name);
        var result = members.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
        return Ok(new Paging<IEnumerable<Member>>
        {
            Data = result,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalPage = (members.Count() / request.PageSize) + 1,
        });
    }
    /// <summary>
    /// Update a member information
    /// </summary>
    /// <param name="member">member model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPut]
    public async Task<ActionResult> Update(Member member)
    {
        var result = await unitOfWork.MemberRepository.UpdateAsync(member);
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
        var result = await unitOfWork.MemberRepository.DeleteAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Import members from csv
    /// </summary>
    /// <returns>true: success, false: fail</returns>
    [HttpGet("import")]
    public async Task<ActionResult> Import()
    {
        var lines = System.IO.File.ReadAllLines("test.csv");
        foreach (var line in lines)
        {
            var array = line.Split(',');
            _ = int.TryParse(array[0], out int id);
            _ = int.TryParse(array[3], out int elo);
            _ = await unitOfWork.MemberRepository.AddAsync(new()
            {
                Id = id,
                Name = array[1],
                Description = array[2],
                Elo = elo
            });
        }
        return Ok();
    }
}
