using BlockingService.Models.Mocks;

namespace BlockingService.Data.AccountMock
{
    public interface IRepositoryAccount
    {
        Account GetAccountByUserName(string username);
        Account Exists(Account account);
    }
}
