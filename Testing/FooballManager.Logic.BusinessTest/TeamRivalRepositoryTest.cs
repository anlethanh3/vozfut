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

        [Fact]
        public void TeamRivalJsonCNTest()
        {
            var teams = new List<TeamRival>
            {
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Khoa Mái",Elo=3},
                        new Member{Name = "Khải",Elo=4},
                        new Member{Name = "Linh",Elo=4},
                        new Member{Name = "Tiến Bịp",Elo=4},
                        new Member{Name = "An Le",Elo=2},
                        new Member{Name = "Đoàn",Elo=3},
                        new Member{Name = "Vũ Huy",Elo=0},
                    },
                    EloSum=20,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Hải Bánh",Elo=4},
                        new Member{Name = "Nhân",Elo=4},
                        new Member{Name = "Việt",Elo=3},
                        new Member{Name = "Giang Trẻ",Elo=3},
                        new Member{Name = "Dũng",Elo=3},
                        new Member{Name = "Kuti",Elo=3},
                        new Member{Name = "Duymax",Elo=0},
                    },
                    EloSum=20,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Ryan",Elo=3},
                        new Member{Name = "Cha Đạt",Elo=3},
                        new Member{Name = "Phú Tê Giác",Elo=5},
                        new Member{Name = "Thuận",Elo=3},
                        new Member{Name = "Đông",Elo=3},
                        new Member{Name = "Hoàng Mèo",Elo=3},
                        new Member{Name = "Tony",Elo=0},
                    },
                    EloSum=20,
                },
                new TeamRival{
                    Players= new List<Member>{
                        new Member{Name = "Thắng Phạm",Elo=3},
                        new Member{Name = "Huân",Elo=3},
                        new Member{Name = "Bí Thư",Elo=3},
                        new Member{Name = "Giang Già",Elo=2},
                        new Member{Name = "Tuấn",Elo=4},
                        new Member{Name = "Thầy",Elo=4},
                        new Member{Name = "Cường",Elo=0},
                    },
                    EloSum=20,
                }
            };
            var json = JsonConvert.SerializeObject(teams);
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test3.json"), json);
            Assert.NotEmpty(json);
        }
    }
}