using FollowingService.Models.Mocks;

namespace FollowingService.Data.AccountMock
{
    public interface IRepositoryAccount
    {
        Account GetAccountByUserName(string username);
        Account Exists(Account account);  
    }
}
