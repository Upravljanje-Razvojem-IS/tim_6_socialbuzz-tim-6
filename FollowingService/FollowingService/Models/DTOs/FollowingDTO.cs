using System;

namespace FollowingService.Models.DTOs
{
    public class FollowingDTO
    {

        /// <summary>
        /// Following ID
        /// </summary>
        public Guid FollowingId { get; set; }

        /// <summary>
        /// Follower ID
        /// </summary>
        public Guid FollowerId { get; set; }

    }
}
