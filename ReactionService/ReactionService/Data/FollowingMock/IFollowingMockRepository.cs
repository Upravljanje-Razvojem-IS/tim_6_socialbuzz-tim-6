using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.FollowingMock
{
    public interface IFollowingMockRepository
    {
        List<Guid> GetUsersIFollow(Guid userId);

        /// <summary>
        /// Check do I follow user
        /// </summary>
        /// <param name="userId">Unique identifier of my (user who send the request) account</param>
        /// <param name="sellerId">Unique identifier of the users for whom I check do I follow him</param>
        /// <returns></returns>
        public bool CheckDoIFollowSeller(Guid userId, Guid sellerId);
    }
}
