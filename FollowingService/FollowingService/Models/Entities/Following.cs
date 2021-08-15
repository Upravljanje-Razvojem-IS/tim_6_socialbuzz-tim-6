using System;
using System.ComponentModel.DataAnnotations;

namespace FollowingService.Model.Entity
{
    /// <summary>
    /// Following model
    /// </summary>
    public class Following
    {
        /// <summary>
        /// Following ID
        /// </summary>
        [Key]
        public Guid FollowingId { get; set; }
        /// <summary>
        /// Follower ID
        /// </summary>
        [Key]
        public Guid FollowerId { get; set; }

    

    }
}
