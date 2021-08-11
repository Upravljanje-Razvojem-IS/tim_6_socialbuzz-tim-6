using FollowingService.Models.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Data.AccountMock
{
    public interface IRepositoryAccount
    {
        Account GetAccountByUserName(string username);
        Account Exists(Account account);  
    }
}
