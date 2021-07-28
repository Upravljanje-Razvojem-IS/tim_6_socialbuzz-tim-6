using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Models.DTOs.Mocks
{
    public class FollowingDto
    {
        /// <summary>
        /// Unique identifier for the following
        /// </summary>
        public int FollowingId;

        /// <summary>
        /// Id of the user who followed another user
        /// </summary>
        public Guid FollowerId;

        /// <summary>
        /// Id of the user who are followed by another user
        /// </summary>
        public Guid FollowedId;
    }
}
