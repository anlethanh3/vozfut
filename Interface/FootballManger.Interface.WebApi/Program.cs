using FootballManager.Data.DataAccess.Contexts;
using FootballManager.Data.DataAccess.Interfaces;
using FootballManager.Data.DataAccess.Repositories;
using FootballManager.Logic.Business;
using FootballManager.Logic.Business.Interfaces;
using FootballManager.Logic.Business.Repositories;

var builder = WebApplication.CreateBuilder(args);

Console.Write("Enter the number of players: ");
int numPlayers = int.Parse(Console.ReadLine());

Console.Write("Enter the number of teams: ");
int numTeams = int.Parse(Console.ReadLine());

RollRepository teamAssignment = new (numPlayers, numTeams);

if (teamAssignment.FindBalancedTeamAssignment())
{
    Console.WriteLine("Balanced team assignment found.");
    List<Player>[] assignedTeams = teamAssignment.GetAssignedTeams();
    BibColor[] teamBibColors = new BibColor[numTeams];

    for (int i = 0; i < numTeams; i++)
    {
        teamBibColors[i] = teamAssignment.ChooseBibColor(i);
        int teamEloSum = assignedTeams[i].Sum(player => player.Elo);
        Console.WriteLine($"Team {i + 1} (Bib Color: {teamAssignment.GetColorName(teamBibColors[i])}, Total Elo: {teamEloSum}):");
        foreach (Player player in assignedTeams[i])
        {
            Console.WriteLine($"- {player.Name} (Elo: {player.Elo})");
        }
    }
}
else
{
    Console.WriteLine("No balanced team assignment found.");
}


// Dependency Injection
builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddDbContext<EntityDbContext>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();