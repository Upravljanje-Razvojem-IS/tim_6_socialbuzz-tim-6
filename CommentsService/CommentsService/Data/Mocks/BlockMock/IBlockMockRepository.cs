using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Data.Mocks.BlockMock
{
    public interface IBlockMockRepository
    {
        public bool CheckIfUserBlocked(int accountID, int blockedAccountID);

        List<int> GetListOfBlockedAccounts(int accountID);
    }
}
