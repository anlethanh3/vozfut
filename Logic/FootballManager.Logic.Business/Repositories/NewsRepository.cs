using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.Entity.Entities;
using FootballManager.Logic.Business.Interfaces;

namespace FootballManager.Logic.Business.Repositories;
public class NewsRepository : INewsRepository
{
    private readonly EntityDbContext entityDbContext;

    public NewsRepository(EntityDbContext entityDbContext)
    {
        this.entityDbContext = entityDbContext;
    }

    public async Task<News?> AddAsync(News model)
    {
        var now = DateTime.Now;
        _ = await entityDbContext.Newses.AddAsync(new()
        {
            Title = model.Title,
            Content = model.Content,
            ImageUris = model.ImageUris,
            CreatedDate = now,
            ModifiedDate = now,
            IsDeleted = false,
        });
        _ = entityDbContext.SaveChanges();
        return model;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member = await GetAsync(id);
        if (member is null)
        {
            return false;
        }
        member.IsDeleted = true;
        member.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }

    public async Task<News?> GetAsync(int id)
    {
        return entityDbContext.Newses.FirstOrDefault(m => m.Id == id);
    }
    
    public async Task<IEnumerable<News>> SearchAsync(string name)
    {
        return entityDbContext.Newses.Where(m => m.Title.ToLower().Contains(name.ToLower()) && !m.IsDeleted).ToList();
    }


    public async Task<IEnumerable<News>> GetAsync()
    {
        return entityDbContext.Newses.Where(x => x.IsDeleted == false).ToList();
    }

    public async Task<bool> UpdateAsync(News model)
    {
        var record = await GetAsync(model.Id);
        if (record is null)
        {
            return false;
        }
        record.Title = model.Title;
        record.Content = model.Content;
        record.ImageUris = model.ImageUris;
        record.ModifiedDate = DateTime.Now;
        _ = entityDbContext.SaveChanges();
        return true;
    }
}