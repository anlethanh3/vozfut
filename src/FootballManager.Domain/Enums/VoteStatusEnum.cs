namespace FootballManager.Domain.Enums
{
    public class VoteStatusEnum : BaseEnumeration<MatchStatusEnum, short>
    {
        public VoteStatusEnum(short value, string name) : base(value, name)
        {
        }

        public static readonly VoteStatusEnum Open = new(1, "Open");
        public static readonly VoteStatusEnum Completed = new(2, "Completed");

        public static IEnumerable<VoteStatusEnum> Gets
            => new[] { Open, Completed };

        public static VoteStatusEnum Get(short id)
            => Gets.FirstOrDefault(x => x.Value == id);
    }
}
