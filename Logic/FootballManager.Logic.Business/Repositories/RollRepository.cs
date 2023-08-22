using FootballManager.Data.Entity;
using FootballManager.Data.Entity.Entities;

namespace FootballManager.Logic.Business.Repositories
{
    public class RollRepository
    {
        private List<Player> players = new(); // List chứa thông tin của các cầu thủ
        private List<Player>[] assignedTeams; // Mảng chứa danh sách cầu thủ của từng đội
        private int numTeams; // Số lượng đội
        public RollRepository(int numTeams, List<Player> members)
        {
            assignedTeams = new List<Player>[numTeams];
            for (int i = 0; i < numTeams; i++)
            {
                assignedTeams[i] = new List<Player>();
            }
            this.numTeams = numTeams;
            this.players.AddRange(members);
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
    }
}