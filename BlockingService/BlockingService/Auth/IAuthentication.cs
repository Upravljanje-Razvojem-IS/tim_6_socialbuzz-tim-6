namespace BlockingService.Auth
{
    public interface IAuthentication
    {
        string GenerateToken(string username, int expireMinutes = 60);
        bool ValidateToken(string token, out string username);
    }
}
