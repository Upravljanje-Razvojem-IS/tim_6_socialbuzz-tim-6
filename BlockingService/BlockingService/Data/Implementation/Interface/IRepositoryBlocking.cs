using BlockingService.Model.Entity;
using BlockingService.Models.Mocks;
using System.Collections.Generic;

namespace BlockingService.Data.Implementation.Interface
{
    public interface IRepositoryBlocking
    {
        Blocking Block(Account blocker, Account blocked);
        Blocking Unblock(Account blocker, Account blocked);
        List<Blocking> GetAllBlockedAccounts(Account account);
        List<Blocking> GetAll();
        bool Find(Account blocker, Account blocked);
    }
}
