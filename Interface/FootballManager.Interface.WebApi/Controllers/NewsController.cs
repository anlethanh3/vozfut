using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.Entity.Entities;
using FootballManager.Data.Entity.Requests;
using FootballManager.Data.Entity.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballManager.Controllers;
/// <summary>
/// News Controller
/// </summary>
[ApiController]
[Route("[controller]"), Authorize]
public class NewsController : ControllerBase
{
    private readonly ILogger<NewsController> logger;
    private readonly IUnitOfWork unitOfWork;
    /// <summary>
    /// News controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public NewsController(ILogger<NewsController> logger, IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
    }
    /// <summary>
    /// Get all News
    /// </summary>
    /// <returns>List news</returns>
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult> GetAll()
    {
        var result = await unitOfWork.NewsRepository.GetAsync();
        return Ok(result);
    }
    /// <summary>
    /// Get one news
    /// </summary>
    /// <param name="id">News id</param>
    /// <returns>A news</returns>
    [HttpGet("{id}"), AllowAnonymous]
    public async Task<ActionResult> Get(int id)
    {
        var result = await unitOfWork.NewsRepository.GetAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Add a news
    /// </summary>
    /// <param name="model">News model</param>
    /// <returns>News info</returns>
    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult> Add(News model)
    {
        var result = await unitOfWork.NewsRepository.AddAsync(model);
        return Ok(result);
    }
    /// <summary>
    /// Update a news information
    /// </summary>
    /// <param name="model">news model</param>
    /// <returns>true: success, false: failure</returns>
    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(News model)
    {
        var result = await unitOfWork.NewsRepository.UpdateAsync(model);
        return Ok(result);
    }
    /// <summary>
    /// Delete a news
    /// </summary>
    /// <param name="id">News id</param>
    /// <returns>true: deleted, false: error</returns>
    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await unitOfWork.NewsRepository.DeleteAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Search news with name
    /// </summary>
    /// <param name="request">Search model</param>
    /// <returns>Paging news</returns>
    [HttpPost("search"), AllowAnonymous]
    public async Task<ActionResult> Search(SearchPagingRequest request)
    {
        var matches = await unitOfWork.NewsRepository.SearchAsync(request.Name);
        var result = matches.Skip(request.PageIndex * request.PageSize).Take(request.PageSize).OrderByDescending(x => x.CreatedDate);
        return Ok(new Paging<IEnumerable<News>>
        {
            Data = result,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalPage = (matches.Count() / request.PageSize) + 1,
        });
    }
}
