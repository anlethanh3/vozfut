using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using FootballManager.Data.Entity.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;
/// <summary>
/// Member Controller
/// </summary>
[ApiController]
[Route("[controller]"), Authorize(Roles = "Admin")]
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
        var result = members.OrderBy(x => x.Name);
        return Ok(result);
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
    [HttpPost("search"), AllowAnonymous]
    public async Task<ActionResult> Search(SearchPagingRequest request)
    {
        var members = await unitOfWork.MemberRepository.SearchAsync(request.Name);
        var result = members.Skip(request.PageIndex * request.PageSize).Take(request.PageSize).OrderBy(x => x.RealName);
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
    /// <summary>
    /// Import members from csv
    /// </summary>
    /// <returns>true: success, false: fail</returns>
    [HttpGet("import2")]
    public async Task<ActionResult> Import2()
    {
        var lines = System.IO.File.ReadAllLines("test3.csv");
        var members = await unitOfWork.MemberRepository.GetAsync();
        var unprocess = new List<string>();
        foreach (var line in lines)
        {
            var array = line.Split(',');
            _ = int.TryParse(array[4], out int speed);
            _ = int.TryParse(array[5], out int stamina);
            _ = int.TryParse(array[6], out int finishing);
            _ = int.TryParse(array[7], out int passing);
            _ = int.TryParse(array[8], out int skill);
            var member = new Member
            {
                Id = 0,
                Name = array[1],
                RealName = array[2],
                Description = array[3],
                Speed = speed,
                Stamina = stamina,
                Finishing = finishing,
                Passing = passing,
                Skill = skill,
            };
            var search = members.FirstOrDefault(x => x.Name.ToLower().Contains(member.Name.ToLower()));
            if (search is not null)
            {
                member.Id = search.Id;
                _ = await unitOfWork.MemberRepository.UpdateAsync(member);
            }
            else
            {
                unprocess.Add(member.Name);
            }
        }
        return Ok(unprocess);
    }
}
