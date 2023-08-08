namespace FootballManager.Domain.Contracts.ResultModels
{
    public interface IApiResult<T>
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }

        T Data { get; set; }

        Exception Exception { get; set; }

        int Code { get; set; }
    }
}
