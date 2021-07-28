using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.BlockingMock
{
    public interface IBlockingMockRepository
    {
        List<Guid> GetUsersIBlocked(Guid userId);

        /// <summary>
        /// Check did I block user
        /// </summary>
        /// <param name="userId">Unique identifier of my (user who send the request) account</param>
        /// <param name="sellerId">Unique identifier of the users for whom I check did I blocked him</param>
        /// <returns></returns>
        public bool CheckDidIBlockSeller(Guid userId, Guid sellerId);
    }
}
