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
        private Guid followingId;

        public Guid FollowingId
        {
            get { return FollowingId; }
            set { FollowingId = value; }
        }

        /// <summary>
        /// Id of the user who followed another user
        /// </summary>
        private Guid followerId;

        public Guid FollowerId
        {
            get { return FollowerId; }
            set { FollowerId = value; }
        }

        /// <summary>
        /// Id of the user who are followed by another user
        /// </summary>
        private Guid followedId;

        public Guid FollowedId
        {
            get { return FollowedId; }
            set { FollowedId = value; }
        }
    }
}
