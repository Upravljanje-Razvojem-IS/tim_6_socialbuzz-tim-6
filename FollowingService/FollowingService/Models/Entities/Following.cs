using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Model.Entity
{
    public class Following
    {
        [Key]
        public Guid FollowingId { get; set; }
        [Key]
        public Guid FollowerId { get; set; }

    

    }
}
