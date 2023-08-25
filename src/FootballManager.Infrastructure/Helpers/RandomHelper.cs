using System.Text;

namespace FootballManager.Infrastructure.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random s_random = new();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[s_random.Next(s.Length)]).ToArray());
        }

        public static int RandomInt(int length)
        {
            const string numbers = "0123456789";

            var sb = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                sb.Append(numbers[s_random.Next(numbers.Length)]);
            }

            return int.Parse(sb.ToString());
        }
    }
}
