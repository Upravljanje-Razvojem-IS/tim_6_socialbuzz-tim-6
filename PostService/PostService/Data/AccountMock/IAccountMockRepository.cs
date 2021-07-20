using PostService.Models.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.AccountMock
{
    public interface IAccountMockRepository
    {
        AccountDto GetAccountByUsername(string username);
    }
}
