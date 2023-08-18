using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballManager.Domain.Entities
{
    public class MemberVote
    {
        [Key]
        [Column(Order = 1)]
        public int MemberId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int VoteId { get; set; }

        public DateTime VoteDate { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        [ForeignKey("VoteId")]
        public virtual Vote Vote { get; set; }
    }
}
