namespace FootballManager.Domain.Exceptions
{
    public class DomainExeption : Exception
    {
        public DomainExeption(string message) : base(message)
        {
        }

        public DomainExeption(string message, Exception innerException) : base(message, innerException)
        { }

        public object Content { get; set; }
    }
}
