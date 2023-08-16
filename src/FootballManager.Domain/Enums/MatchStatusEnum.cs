namespace FootballManager.Domain.Enums
{
    public class MatchStatusEnum : BaseEnumeration<MatchStatusEnum, short>
    {
        public MatchStatusEnum(short value, string name) : base(value, name)
        {
        }

        public static readonly MatchStatusEnum ComingSoon = new(1, "ComingSoon");
        public static readonly MatchStatusEnum Completed = new(2, "Completed");
        public static readonly MatchStatusEnum Cancelled = new(3, "Cancelled");

        public static IEnumerable<MatchStatusEnum> Gets
            => new[] { ComingSoon, Completed, Cancelled };

        public static MatchStatusEnum Get(short id)
            => Gets.FirstOrDefault(x => x.Value == id);
    }
}
