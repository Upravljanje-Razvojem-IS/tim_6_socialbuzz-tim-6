using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
