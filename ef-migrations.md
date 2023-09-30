# Use this command to database migrations
1. dotnet ef migrations add AddNews -p ..\..\Data\FootballManager.Data.DataAccess\ -c FootballManager.Data.DataAccess.Contexts.EntityDbContext -o ..\..\Data\FootballManager.Data.DataAccess\Migrations\

2. dotnet ef database update AddNews -p ..\..\Data\FootballManager.Data.DataAccess\ -c FootballManager.Data.DataAccess.Contexts.EntityDbContext