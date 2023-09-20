# Use this command to database migrations
1. dotnet ef migrations add PolicyPermission -p ..\..\Data\FootballManager.Data.DataAccess\ -c FootballManager.Data.DataAccess.Contexts.EntityDbContext -o ..\..\Data\FootballManager.Data.DataAccess\Migrations\

2. dotnet ef database update PolicyPermission -p ..\..\Data\FootballManager.Data.DataAccess\ -c FootballManager.Data.DataAccess.Contexts.EntityDbContext