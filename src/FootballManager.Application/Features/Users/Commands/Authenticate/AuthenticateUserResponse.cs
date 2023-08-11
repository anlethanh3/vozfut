namespace FootballManager.Application.Features.Users.Commands.Authenticate
{
    public record AuthenticateUserResponse(string AccessToken, string TokenType, int ExpiredIn);
}
