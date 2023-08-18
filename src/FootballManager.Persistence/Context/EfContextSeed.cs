using FootballManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FootballManager.Persistence.Context
{
    public class EfContextSeed
    {
        private static readonly string s_systemString = "System";
        private static readonly DateTime s_dateNow = DateTime.UtcNow;

        public static async Task SeedAsync(EfDbContext context, ILogger<EfContextSeed> logger)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(GetPreConfigureUsers());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed table {Table} database associted with context {Dbcontext}", "Users", typeof(EfContextSeed).Name);
            }

            if (!context.Members.Any())
            {
                context.Members.AddRange(GetPreConfigureMembers());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed table {Table} database associted with context {DbContext}", "Members", typeof(EfDbContext).Name);
            }

            if (!context.Positions.Any())
            {
                context.Positions.AddRange(GetPreConfigurePosition());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed data for table {Table} with context {DbContext}", "Positions", typeof(EfDbContext).Name);
            }
        }

        private static List<User> GetPreConfigureUsers()
            => new()
            {
                new User
                {
                    Username = "tamn.chichi",
                    PasswordHash = GeneratePasswordHash(null, "Tamn0310@"),
                    Email = "tamn0310@gmail.com",
                    Name = "Tâm",
                    IsAdmin = true,
                    MemberId = 0,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "system"
                },
                new User
                {
                    Username = "anle",
                    PasswordHash = GeneratePasswordHash(null, "12345678"),
                    Email = "an.lethanh3@gmail.com",
                    Name = "AnLe",
                    IsAdmin = true,
                    MemberId = 0,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "system"
                }
            };

        private static List<Member> GetPreConfigureMembers()
            => new()
            {
                new Member
                {
                     Name = "Nguyễn Tâm",
                     Elo = 3,
                     CreatedBy = "System",
                     CreatedDate = DateTime.UtcNow,
                     DeletedDate = null,
                     IsDeleted = false,
                     Description = "Description",
                     ModifiedBy = null,
                     ModifiedDate = null
                }
            };

        private static List<Position> GetPreConfigurePosition()
        => new()
        {
            new Position
            {
                Name = "Goalkeeper",
                Code = "GK",
                Description = "Thủ môn",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
             {
                 Name = "Left forward",
                 Code = "LF",
                 Description = "Tiền đạo cánh trái, trong sơ đồ 2 hoặc 3 tiền đạo",
                 CreatedBy = s_systemString,
                 CreatedDate = s_dateNow
             },
             new Position
            {
                Name = "Right forward",
                Code = "RF",
                Description = "Tiền đạo cánh phải, trong sơ đồ 2 hoặc 3 tiền đạo",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Centre Forward",
                Code = "CF",
                Description = "Tiền đạo trung tâm, trong sơ đồ 4-3-3",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Sweeper / Libero",
                Code = "SW",
                Description = "Trung vệ thòng, đá thấp nhất trong 3 trung vệ, ví dụ trong sơ đồ 3-5-2",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Striker",
                Code = "ST",
                Description = "Tiền đạo cắm/Trung phong, trong sơ đồ chơi 1 tiền đạo duy nhất, ví dụ 4-3-2-1",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Centre Back / Centre Defender",
                Code = "CB",
                Description = "Trung vệ",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Left Back / Left Defender",
                Code = "LB",
                Description = "Hậu vệ trái",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Right Back / Right Defender",
                Code = "RB",
                Description = "Hậu vệ phải",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Right Sideback",
                Code = "RS",
                Description = "Hậu vệ phải",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Left Sideback",
                Code = "LS",
                Description = "Hậu vệ trái",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Left Midfielder",
                Code = "LM",
                Description = "Tiền vệ trái",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Right Midfielder",
                Code = "RM",
                Description = "Tiền vệ phải",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Centre Midfielder",
                Code = "CM",
                Description = "Tiền vệ trung tâm",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Left  Wide Back",
                Code = "LWB",
                Description = "Hậu vệ chạy cánh trái, trong sơ đồ 5 hậu vệ như 5-3-2.",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Right Wide Back",
                Code = "RWB",
                Description = "Hậu vệ chạy cánh phải, trong sơ đồ 5 hậu vệ như 5-3-2.",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Left Wide Midfielder/Left Winger",
                Code = "LWM/LW",
                Description = "Tiền vệ chạy cánh trái, có trong sơ đồ 4-5-1.",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Right Wide Midfielder/Right Winger",
                Code = "RWM/RW",
                Description = "Tiền vệ chạy cánh phải, có trong sơ đồ 4-5-1.",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Attacking Midfielder",
                Code = "AM",
                Description = "Tiền vệ tấn công",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Defensive Midfielder",
                Code = "DM",
                Description = "Tiền vệ trụ / Tiền vệ phòng ngự,  trong sơ đồ 4-1-4-1.",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },new Position
            {
                Name = "Right Defensive Midfielder",
                Code = "RDM",
                Description = "Tiền vệ phòng ngự phải",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
             new Position
            {
                Name = "Left Defensive Midfielder",
                Code = "LDM",
                Description = "Tiền vệ phòng ngự trái",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
              new Position
            {
                Name = "Right Central Defensive Midfielder",
                Code = "RCDM",
                Description = "Tiền vệ phòng ngự trung tâm những chếch về cánh phải",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },

               new Position
            {
                Name = "Left Central Defensive Midfielder",
                Code = "LCDM",
                Description = "Tiền vệ phòng ngự trung tâm những chếch về cánh trái",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
                new Position
            {
                Name = "Centre Defensive Midfielder",
                Code = "CDM",
                Description = "Tiền vệ trụ / Tiền vệ phòng ngự, trong sơ đồ 4-2-3-1.",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
                 new Position
            {
                Name = "Central Attacking Midfielder ",
                Code = "CAM",
                Description = "Tiền vệ tấn công trung tâm",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
                  new Position
            {
                Name = "Right Attacking Sidfielder",
                Code = "RAM",
                Description = "Tiền vệ tấn công cánh phải",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
                    new Position
            {
                Name = "Right Central Attacking Midfielder",
                Code = "RCAM",
                Description = "Tiền vệ tấn công trung tâm nhưng chếch về cánh phải",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
                      new Position
            {
                Name = "Left Attacking Midfielder",
                Code = "LAM",
                Description = "Tiền vệ tấn công cánh trái",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            },
                        new Position
            {
                Name = "Left Central Attacking Midfielder",
                Code = "LCAM",
                Description = "Tiền vệ tấn công trung tâm nhưng chếch về cánh trái",
                CreatedBy = s_systemString,
                CreatedDate = s_dateNow
            }
        };

        private static string GeneratePasswordHash(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            var hash = passwordHasher.HashPassword(user, $"{password}");
            return hash;
        }
    }
}
