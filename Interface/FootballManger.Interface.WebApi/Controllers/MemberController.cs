using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FootballManger.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberController : ControllerBase
{
    private readonly ILogger<MemberController> logger;
    private readonly IUnitOfWork unitOfWork;

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
    [HttpPost]
    public async Task<ActionResult> Add(Member member)
    {
        var result = await unitOfWork.MemberRepository.AddAsync(member);
        return Ok(result);
    }
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
