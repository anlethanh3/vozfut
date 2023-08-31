using System;
using FootballManager.Data.Entity.Entities;
using Newtonsoft.Json;
using Xunit;

namespace FooballManager.Logic.BusinessTest
{
    public class TeamRivalRepositoryTest
    {
        [Fact]
        public void TeamRivalJsonT5Test()
        {
            var teams = new List<TeamRival>
            {
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Tiến Bịp",Elo=4},
                        new Member{Name = "Dũng",Elo=3},
                        new Member{Name = "Kid Bụi Đời",Elo=3},
                        new Member{Name = "Đông",Elo=3},
                        new Member{Name = "Dương Tuấn Việt",Elo=2},
                    },
                    EloSum=15,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Hoàng Mèo",Elo=4},
                        new Member{Name = "Ryan",Elo=3},
                        new Member{Name = "Sang - Tonyknight",Elo=3},
                        new Member{Name = "Seen No Rep",Elo=2},
                        new Member{Name = "Mầu Thanh Hùng",Elo=1},
                    },
                    EloSum=13,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Tín Huỳnh",Elo=4},
                        new Member{Name = "Hà Huy",Elo=4},
                        new Member{Name = "Khoa Nhớt",Elo=3},
                        new Member{Name = "An Le",Elo=2},
                        new Member{Name = "Nguyễn Lam Giang",Elo=1},
                    },
                    EloSum=14,
                }
            };
            var json = JsonConvert.SerializeObject(teams);
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json"), json);
            Assert.NotEmpty(json);
        }

        [Fact]
        public void TeamRivalJsonT6Test()
        {
            var teams = new List<TeamRival>
            {
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "TD",Elo=4},
                        new Member{Name = "Dũng",Elo=3},
                        new Member{Name = "Hữu Minh",Elo=3},
                        new Member{Name = "Khoa Nhớt",Elo=3},
                        new Member{Name = "Mầu Thanh Hùng",Elo=1},
                    },
                    EloSum=14,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Anh Tuấn",Elo=4},
                        new Member{Name = "Hải Zombie",Elo=3},
                        new Member{Name = "Giang Phạm",Elo=3},
                        new Member{Name = "An Le",Elo=2},
                        new Member{Name = "Phạm Hải Anh Duy",Elo=2},
                    },
                    EloSum=14,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Thắng Phạm",Elo=4},
                        new Member{Name = "Châu Toàn",Elo=3},
                        new Member{Name = "Ryan",Elo=3},
                        new Member{Name = "Seen No Rep",Elo=2},
                        new Member{Name = "Dương Tuấn Việt",Elo=2},
                    },
                    EloSum=14,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Lê Châu Đạt",Elo=4},
                        new Member{Name = "Văn Tùng",Elo=3},
                        new Member{Name = "Nguyễn Tâm",Elo=3},
                        new Member{Name = "Trần Sang",Elo=2},
                        new Member{Name = "Sơn Nguyễn (Mai Hoàng)",Elo=1},
                    },
                    EloSum=13,
                }
            };
            var json = JsonConvert.SerializeObject(teams);
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test2.json"), json);
            Assert.NotEmpty(json);
        }
    }
}