using BlockingService.Model.Entity;
using BlockingService.Models.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
