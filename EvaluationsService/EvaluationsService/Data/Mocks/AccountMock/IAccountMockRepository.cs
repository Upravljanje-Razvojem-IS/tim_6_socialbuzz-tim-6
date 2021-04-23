using EvaluationsService.Model.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Data.Mocks.AccountMock
{
    public interface IAccountMockRepository
    {
        String getUsernameByID(int ID);

        AccountDto GetAccountByID(int ID);
    }
}
