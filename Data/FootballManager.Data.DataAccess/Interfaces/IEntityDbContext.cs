using FootballManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Data.DataAccess.Interfaces;

public interface IEntityDbContext
{
    public DbContext DbContext { get; }
    public DbSet<Member> Members { get; set; }
}
