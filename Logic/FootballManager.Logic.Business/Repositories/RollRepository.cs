namespace FootballManager.Logic.Business.Repositories
{
    public class RollRepository
    {
        private List<Player> players = new List<Player>(); // List chứa thông tin của các cầu thủ
        private List<Player>[] assignedTeams; // Mảng chứa danh sách cầu thủ của từng đội
        private int numTeams; // Số lượng đội

        public RollRepository(int numPlayers, int numTeams)
        {
            assignedTeams = new List<Player>[numTeams];
            for (int i = 0; i < numTeams; i++)
            {
                assignedTeams[i] = new List<Player>();
            }
            this.numTeams = numTeams;

            for (int i = 0; i < numPlayers; i++)
            {
                Console.Write($"Enter name and elo for player {i + 1}: ");
                string[] playerData = Console.ReadLine().Split(' ');
                if (playerData.Length != 2 || !int.TryParse(playerData[1], out int elo))
                {
                    Console.WriteLine("Invalid input. Please provide name and elo separated by space.");
                    i--;
                    continue;
                }
                players.Add(new Player { Name = playerData[0], Elo = elo });
            }
        }

        private bool IsBalanced(List<Player>[] teams)
        {
            int sum = teams.Sum(t => t.Sum(p => p.Elo));
            int avg = sum / numTeams;

            foreach (List<Player> team in teams)
            {
                int teamSum = team.Sum(p => p.Elo);
                if (Math.Abs(teamSum - avg) > 1)
                    return false;
            }

            return true;
        }

        private bool AssignTeams(int playerIndex)
        {
            if (playerIndex == players.Count)
            {
                return IsBalanced(assignedTeams);
            }

            for (int i = 0; i < numTeams; i++)
            {
                assignedTeams[i].Add(players[playerIndex]);
                if (AssignTeams(playerIndex + 1))
                {
                    return true;
                }
                assignedTeams[i].RemoveAt(assignedTeams[i].Count - 1);
            }

            return false;
        }

        public bool FindBalancedTeamAssignment()
        {
            return AssignTeams(0);
        }

        public List<Player>[] GetAssignedTeams()
        {
            return assignedTeams;
        }

        public BibColor ChooseBibColor(int teamIndex)
        {
            switch (teamIndex)
            {
                case 0: return BibColor.Orange;
                case 1: return BibColor.Red;
                case 2: return BibColor.Blue;
                case 3: return BibColor.Green;
                default: return BibColor.Orange; // Fallback color
            }
        }

        public string GetColorName(BibColor color)
        {
            return color switch
            {
                BibColor.Orange => "Orange",
                BibColor.Red => "Red",
                BibColor.Blue => "Blue",
                BibColor.Green => "Green",
                _ => "Unknown",
            };
        }
    }
}